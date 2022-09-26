using System.Globalization;

namespace Crosscutting.Common.Tools.DataType
{
    public class FieldValueOrder
    {
        public string Key { set; get; }

        public long KeyInt
        {
            set { Key = value.ToString(CultureInfo.InvariantCulture); }
            get { return int.Parse(Key,CultureInfo.InvariantCulture); }
        }

        public string Value { set; get; }
        public int Order { set; get; }
        public int Applicant { set; get; }
        public byte[] Bytes { set; get; }
    }
}