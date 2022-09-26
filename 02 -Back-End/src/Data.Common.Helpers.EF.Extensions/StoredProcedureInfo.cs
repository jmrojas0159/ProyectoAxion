using System.Data.SqlClient;

namespace Data.Common.Helpers.EF.Extensions
{
    internal class StoredProcedureInfo
    {
        internal string Sql { get; set; }
        internal SqlParameter[] SqlParameters { get; set; }
    }
}