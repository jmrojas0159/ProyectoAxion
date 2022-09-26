using Crosscutting.Common.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Crosscutting.Common.Tools.Extensions
{
    public static class StringExtensions
    {
        public static string ToLiteral(this string input)
        {
            return HttpUtility.HtmlEncode(input);
        }
    }

    public static class ByteExtensions
    {
        public enum UnitSize
        {
            By = 0,
            Kb = 1,
            Mb = 2,
            Gb = 3,
            Tb = 4,
            Pb = 5,
            Eb = 6


        }
        public static double BytesToString(long byteCount, UnitSize unitSize)
        {

            if (byteCount == 0)
            {
                return 0;
            }
            long bytes = Math.Abs(byteCount);
            double num = Math.Round(bytes / Math.Pow(1024, (int)unitSize), 1);
            return (Math.Sign(byteCount) * num);
        }
    }

    public static class DateExtensions
    {
        public static string ToAfinFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH\\:mm\\:ss");
        }
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }

    public static class ObjectExtensions
    {
        public static object ChangeType(this object value, Type conversionType)
        {
            if (conversionType == null) throw new ArgumentNullException(nameof(conversionType));
            if (conversionType == null)
            {
                throw new ArgumentNullException(nameof(conversionType));
            }
            if (!conversionType.IsGenericType || conversionType.GetGenericTypeDefinition() != typeof(Nullable<>))
                return Convert.ChangeType(value, conversionType, CultureInfo.CurrentCulture);
            if (value == null)
            {
                return null;
            }
            var nullableConverter = new NullableConverter(conversionType);
            conversionType = nullableConverter.UnderlyingType;
            return Convert.ChangeType(value, conversionType, CultureInfo.CurrentCulture);
        }

        public static T MapDictionaryToObject<T>(T obj, IEnumerable<FieldValueType> fieldValueTypes)
                    where T : class
        {
            var valueTypes = fieldValueTypes as IList<FieldValueType> ?? fieldValueTypes.ToList();
            var names = valueTypes.Select(s => s.Key).Distinct().ToList();
            var propertiesmach = obj.GetType().GetProperties().ToList().Where(s => names.Contains(s.Name));
            foreach (var info in propertiesmach)
            {
                var item = valueTypes.FirstOrDefault(s => s.Key == info.Name);
                if (item == null) continue;
                if (info.CanWrite)
                {
                    info.SetValue(obj, item.Value.ChangeType(info.PropertyType), null);
                }
                //obj.GetType().GetProperty(info.Name).SetValue(info.Name, item.Value, null);
            }

            return obj;
        }
    }
}