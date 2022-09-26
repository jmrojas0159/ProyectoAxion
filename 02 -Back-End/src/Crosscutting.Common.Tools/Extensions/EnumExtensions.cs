using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Crosscutting.Common.Tools.DataType;

namespace Crosscutting.Common.Tools.Extensions
{
    public static class EnumExtension
    {
        public static string GetMappingForServiceValue(this Enum value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            string output = null;
            var type = value.GetType();

            var fi = type.GetField(value.ToString());
            var attrs = fi?.GetCustomAttributes(typeof(MappingForServiceValue), false) as MappingForServiceValue[];
            if (attrs?.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }

        public static long GetMappingToItemListValue(this Enum value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            long output = 0;
            var type = value.GetType();

            // Check first in our cached results...

            // Look for our 'StringValueAttribute'

            // in the field's custom attributes
            var fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(MappingToItemListValue), false) as MappingToItemListValue[];
            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        public static IEnumerable<T> GetAllOptions<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
        public static T GetEnumFromFriendlyDescriptionValue<T>(string value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(FriendlyDescriptionValue)) as FriendlyDescriptionValue;
                if (attribute != null)
                {
                    if (attribute.Value == value)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "value");
            // or return default(T);
        }
        public static T GetEnumFromMappingForServiceValue<T>(string value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(MappingForServiceValue)) as MappingForServiceValue;
                if (attribute != null)
                {
                    if (attribute.Value == value)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }

        public static string GetStringValue(this Enum value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            string output = null;
            var type = value.GetType();

            var fi = type.GetField(value.ToString());
            var attrs = fi?.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs?.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }

        public static string GetFriendlyDescriptionValue(this Enum value)
        {
            string output = null;
            var type = value.GetType();

            // Check first in our cached results...
            // Look for our 'StringValueAttribute' 
            // in the field's custom attributes
            FieldInfo fi = type.GetField(value.ToString());
            var attrs = fi?.GetCustomAttributes(typeof(FriendlyDescriptionValue), false) as FriendlyDescriptionValue[];
            if (attrs?.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}