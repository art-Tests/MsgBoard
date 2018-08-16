using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MsgBoard.Services.Interface;

namespace MsgBoard.Services.Factory
{
    public class ConnectionFactory : IConnectionFactory
    {
        private static readonly string CurrectDbName = ConfigurationManager.AppSettings["currectDb"];
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings[CurrectDbName].ConnectionString;

        public IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}