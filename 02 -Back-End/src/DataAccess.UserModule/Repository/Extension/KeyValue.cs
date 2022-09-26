using Data.Common.Helpers.EF.Extensions;

namespace DataAccess.UserModule.Repository.Extension
{
    [UserDefinedTableType("dbo.KeyValue")]
    public class KeyValue
    {
        [UserDefinedTableTypeColumn(1)]
        public long Id { get; set; }

        [UserDefinedTableTypeColumn(2)]
        public string Value { get; set; }

        [UserDefinedTableTypeColumn(3)]
        public byte[] Bytes { get; set; }

        [UserDefinedTableTypeColumn(4)]
        public int Applicant { get; set; }

        [UserDefinedTableTypeColumn(5)]
        public int CollectionNumber { get; set; }
    }
}