﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Data.Common.Helpers.EF.Extensions
{
    /// <summary>
    /// Extension methods for the Entity Framework Database class.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Executes the specified stored procedure against a database.
        /// </summary>
        /// <param name="database">The database to execute against.</param>
        /// <param name="storedProcedure">The stored procedure to execute.</param>
        public static void ExecuteStoredProcedure(this Database database, object storedProcedure)
        {
            if (storedProcedure == null)
                throw new ArgumentNullException(nameof(storedProcedure));

            var info = StoredProcedureParser.BuildStoredProcedureInfo(storedProcedure);

            // ReSharper disable once CoVariantArrayConversion
            database.ExecuteSqlCommand(info.Sql, info.SqlParameters);

            SetOutputParameterValues(info.SqlParameters, storedProcedure);
        }

        /// <summary>
        /// Executes the specified stored procedure against a database and returns an enumerable of T
        /// representing the data returned.
        /// </summary>
        /// <typeparam name="T">Type of the data returned from the stored procedure.</typeparam>
        /// <param name="database">The database to execute against.</param>
        /// <param name="storedProcedure">The stored procedure to execute.</param>
        /// <returns></returns>
        public static IEnumerable<T> ExecuteStoredProcedure<T>(this Database database, object storedProcedure)
        {
            if (storedProcedure == null)
                throw new ArgumentNullException(nameof(storedProcedure));

            var info = StoredProcedureParser.BuildStoredProcedureInfo(storedProcedure);

            // ReSharper disable once CoVariantArrayConversion
            var result = database.SqlQuery<T>(info.Sql, info.SqlParameters).ToList();

            SetOutputParameterValues(info.SqlParameters, storedProcedure);

            return result;
        }

        private static void SetOutputParameterValues(IEnumerable<SqlParameter> sqlParameters, object storedProcedure)
        {
            foreach (var sqlParameter in sqlParameters.Where(p => p.Direction != ParameterDirection.Input))
            {
                var propertyInfo = GetMatchingProperty(storedProcedure, sqlParameter);

                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(storedProcedure,
                        sqlParameter.Value == DBNull.Value ?
                        GetDefault(propertyInfo.PropertyType) :
                        sqlParameter.Value, null);
                }
            }
        }

        private static PropertyInfo GetMatchingProperty(object storedProcedure, SqlParameter parameter)
        {
            foreach (var propertyInfo in storedProcedure.GetType().GetProperties().Where(p => p.HasAttribute<StoredProcedureParameterAttribute>()))
            {
                var name = StoredProcedureParserHelper.GetParameterName(propertyInfo);

                if (parameter.ParameterName.Substring(1) == name)
                    return propertyInfo;
            }

            return null;
        }

        private static object GetDefault(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}