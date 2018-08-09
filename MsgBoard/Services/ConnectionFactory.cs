using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MsgBoard.Services
{
    public class ConnectionFactory
    {
        private static readonly string _currectDbName = ConfigurationManager.AppSettings["currectDb"];
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings[_currectDbName].ConnectionString;

        public static IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}