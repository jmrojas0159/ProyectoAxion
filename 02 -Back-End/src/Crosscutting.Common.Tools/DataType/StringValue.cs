using System;

namespace Crosscutting.Common.Tools.DataType
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class StringValue : Attribute
    {
        public StringValue(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}