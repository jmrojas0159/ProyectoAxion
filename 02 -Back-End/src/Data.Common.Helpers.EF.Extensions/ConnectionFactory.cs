using System.Data;
using System.Data.Entity.Infrastructure;

namespace Data.Common.Helpers.EF.Extensions
{
    public static class ConnectionFactory
    {
        public static IDbConnection GetConnection(string nameOrConnectionString)
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[nameOrConnectionString]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString)) connectionString = nameOrConnectionString;
            var factory = new SqlConnectionFactory();
            return factory.CreateConnection(connectionString);
        }

        public static void TryCloseConnection(this IDbConnection connection)
        {
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }
    }
}