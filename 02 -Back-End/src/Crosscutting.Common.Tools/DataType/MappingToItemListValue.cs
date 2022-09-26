using System;

namespace Crosscutting.Common.Tools.DataType
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MappingToItemListValue : Attribute
    {
        public MappingToItemListValue(long value)
        {
            Value = value;
        }

        public long Value { get; }
    }
}