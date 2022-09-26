using System;

namespace Crosscutting.Common.Tools.DataType
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class MappingForServiceValue : Attribute
    {
        public MappingForServiceValue(string value)
        {
            Value = value;
        }
        public string Value { get; }
    }
    
}
