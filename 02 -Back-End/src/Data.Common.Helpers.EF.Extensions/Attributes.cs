using System;
using System.Linq;
using System.Reflection;

namespace Data.Common.Helpers.EF.Extensions
{
    internal static class Attributes
    {
        public static T GetAttribute<T>(Type fromType) where T : Attribute
        {
            var attributes = fromType.GetCustomAttributes(typeof(T), false);

            return GetAttribute<T>(attributes);
        }

        public static T GetAttribute<T>(PropertyInfo propertyInfo) where T : Attribute
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(T), false);

            return GetAttribute<T>(attributes);
        }

        public static bool HasAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            var attr = GetAttribute<T>(propertyInfo);

            return attr != null;
        }

        private static T GetAttribute<T>(object[] attributes) where T : Attribute
        {
            if (!attributes.Any())
                return null;

            var attribute = (T)attributes.First();

            return attribute;
        }
    }
}