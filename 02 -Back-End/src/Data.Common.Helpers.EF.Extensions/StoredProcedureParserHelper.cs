using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Data.Common.Helpers.EF.Extensions
{
    internal class StoredProcedureParserHelper
    {
        public static string GetParameterName(PropertyInfo propertyInfo)
        {
            var rValue = propertyInfo.Name;

            // grab the name of the parameter from the attribute, if it's defined.
            var attribute = Attributes.GetAttribute<StoredProcedureParameterAttribute>(propertyInfo);
            if (!string.IsNullOrEmpty(attribute?.ParameterName))
                rValue = attribute.ParameterName;

            return rValue;
        }

        public static string GetUserDefinedTableType(PropertyInfo propertyInfo)
        {
            var collectionType = GetCollectionType(propertyInfo.PropertyType);
            var attribute = Attributes.GetAttribute<UserDefinedTableTypeAttribute>(collectionType);

            if (attribute == null)
                throw new InvalidOperationException(
                    $"{propertyInfo.PropertyType} has not been decorated with UserDefinedTableTypeAttribute.");

            return attribute.Name;
        }

        public static Type GetCollectionType(Type type) =>
            type.IsGenericType ? (from interfaceType in type.GetInterfaces() where interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IList<>) select interfaceType.GetGenericArguments()[0]).FirstOrDefault() : null;

        public static object GetUserDefinedTableValue(PropertyInfo propertyInfo, object storedProcedure)
        {
            var enumerableType = GetCollectionType(propertyInfo.PropertyType);
            var propertyValue = propertyInfo.GetValue(storedProcedure, null);

            var generator = new UserDefinedTableGenerator(enumerableType, propertyValue);

            var table = generator.GenerateTable();

            return table;
        }

        public static bool IsUserDefinedTableParameter(PropertyInfo propertyInfo)
        {
            var collectionType = GetCollectionType(propertyInfo.PropertyType);

            return collectionType != null;
        }

        public static bool ParameterIsMandatory(StoredProcedureParameterOptions options)
        {
            return (options & StoredProcedureParameterOptions.Mandatory) ==
                   StoredProcedureParameterOptions.Mandatory;
        }
    }
}