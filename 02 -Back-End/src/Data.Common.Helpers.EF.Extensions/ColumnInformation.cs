using System.Reflection;

namespace Data.Common.Helpers.EF.Extensions
{
    internal class ColumnInformation
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public PropertyInfo Property { get; set; }
    }
}