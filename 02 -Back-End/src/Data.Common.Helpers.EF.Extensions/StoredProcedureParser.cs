using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Common.Helpers.EF.Extensions
{
    internal class StoredProcedureParser
    {
        public static StoredProcedureInfo BuildStoredProcedureInfo(object storedProcedure)
        {
            var parameterInfo = BuildStoredProcedureParameterInfo(storedProcedure);

            var sql = BuildSql(storedProcedure, parameterInfo);
            var sqlParameters = BuildSqlParameters(storedProcedure, parameterInfo);

            var info = new StoredProcedureInfo()
            {
                Sql = sql,
                SqlParameters = sqlParameters
            };

            return info;
        }

        public static string GetStoreProcedureName(object storedProcedure)
        {
            var attribute = Attributes.GetAttribute<StoredProcedureAttribute>(storedProcedure.GetType());

            if (attribute == null)
                throw new InvalidOperationException(
                    $"{storedProcedure.GetType()} is not decorated with StoredProcedureAttribute.");

            return attribute.Name;
        }

        public static Collection<StoredProcedureParameterInfo> BuildStoredProcedureParameterInfo(object storedProcedure)
        {
            var parameters = new Collection<StoredProcedureParameterInfo>();

            foreach (var propertyInfo in storedProcedure.GetType().GetProperties())
            {
                var attribute = Attributes.GetAttribute<StoredProcedureParameterAttribute>(propertyInfo);

                if (attribute == null) continue;
                var parameterName = StoredProcedureParserHelper.GetParameterName(propertyInfo);

                var isUserDefinedTableParameter = StoredProcedureParserHelper.IsUserDefinedTableParameter(propertyInfo);
                var isMandatory = StoredProcedureParserHelper.ParameterIsMandatory(attribute.Options);

                parameters.Add(new StoredProcedureParameterInfo
                {
                    Name = parameterName,
                    IsUserDefinedTable = isUserDefinedTableParameter,
                    IsMandatory = isMandatory,
                    SqlDataType = attribute.DataType,
                    PropertyInfo = propertyInfo,
                    Direction = attribute.Direction,
                    Size = attribute.Size
                });
            }

            return parameters;
        }

        private static string BuildSql(object storedProcedure, IEnumerable<StoredProcedureParameterInfo> parameterInfos)
        {
            var storedProcedureName = GetStoreProcedureName(storedProcedure);

            var execSql = $"EXEC {storedProcedureName} ";
            var parameterSql = string.Join(",", parameterInfos.Select(pi => string.Format("@{0} = @{0} {1}", pi.Name, pi.Direction == ParameterDirection.Input ? string.Empty : "out")));

            return execSql + parameterSql;
        }

        private static SqlParameter[] BuildSqlParameters(object storedProcedure, IEnumerable<StoredProcedureParameterInfo> parameterInfos)
        {
            var sqlParams = new List<SqlParameter>();

            foreach (var p in parameterInfos)
            {
                var propertyValue = p.PropertyInfo.GetValue(storedProcedure, null);

                var value = p.IsUserDefinedTable
                                         ? StoredProcedureParserHelper.GetUserDefinedTableValue(p.PropertyInfo, storedProcedure)
                                         : propertyValue;

                var sqlParameter = GenerateSqlParameter(p.Name,
                                                                 value,
                                                                 p.IsMandatory,
                                                                 p.Size,
                                                                 p.IsUserDefinedTable,
                                                                 p.IsUserDefinedTable ?
                                                                                    StoredProcedureParserHelper.GetUserDefinedTableType(p.PropertyInfo) : null,
                                                                p.SqlDataType,
                                                                p.Direction);

                sqlParams.Add(sqlParameter);
            }

            return sqlParams.ToArray();
        }

        private static SqlParameter GenerateSqlParameter(string parameterName, object paramValue, bool mandatory, int size,
                                           bool isUserDefinedTableParameter, string udtType, SqlDbType dataType, ParameterDirection direction)
        {
            var sqlParameter = new SqlParameter("@" + parameterName, paramValue ?? DBNull.Value)
            {
                Direction = direction,
                IsNullable = !mandatory,
                Size = SetSize(size, direction),
            };

            if (isUserDefinedTableParameter)
                sqlParameter.TypeName = udtType;
            else
                sqlParameter.SqlDbType = dataType;

            return sqlParameter;
        }

        private static int SetSize(int size, ParameterDirection direction)
        {
            if (direction != ParameterDirection.Input && size == 0) //output parameter
                return -1;

            return size;
        }
    }
}