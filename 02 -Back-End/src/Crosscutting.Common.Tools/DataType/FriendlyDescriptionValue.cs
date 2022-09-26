using System;

namespace Crosscutting.Common.Tools.DataType
{
    public class FriendlyDescriptionValue : Attribute
    {
        public FriendlyDescriptionValue(string value)
        {
            Value = value;
        }
        public string Value { get; }
    }
}