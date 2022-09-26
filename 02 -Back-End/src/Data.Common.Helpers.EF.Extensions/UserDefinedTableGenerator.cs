using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Data.Common.Helpers.EF.Extensions
{
    public class UserDefinedTableGenerator
    {
        private readonly Type _type;
        private readonly object _value;

        public UserDefinedTableGenerator(Type type, object value)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _type = type;
            _value = value;
        }

        public DataTable GenerateTable()
        {
            using (var dt = new DataTable())
            {
                var columns = GetColumnInformation();

                AddColumns(columns, dt);
                AddRows(columns, dt);

                return dt;
            }
        }

        internal static void AddColumns(List<ColumnInformation> columns, DataTable dt)
        {
            foreach (var column in columns)
            {
                var type = column.Property.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GetGenericArguments()[0];
                }

                dt.Columns.Add(column.Name, type);
            }
        }

        internal void AddRows(List<ColumnInformation> columns, DataTable dt)
        {
            foreach (var o in (IEnumerable)_value)
            {
                var row = dt.NewRow();
                dt.Rows.Add(row);

                foreach (var column in columns)
                {
                    var value = column.Property.GetValue(o, null);
                    row.SetField(column.Name, value);
                }
            }
        }

        private List<ColumnInformation> GetColumnInformation()
        {
            var columns = (from propertyInfo in _type.GetProperties()
                           let attribute = Attributes.GetAttribute<UserDefinedTableTypeColumnAttribute>(propertyInfo)
                           where attribute != null
                           select new ColumnInformation
                           {
                               Name = attribute.Name ?? propertyInfo.Name,
                               Property = propertyInfo,
                               Order = attribute.Order
                           }).ToList();

            return columns.OrderBy(info => info.Order).ToList();
        }
    }
}