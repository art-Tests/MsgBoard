using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MsgBoard.Models.Interface;

namespace MsgBoard.Services
{
    public class ConnectionFactory : IConnectionFactory
    {
        private static readonly string _currectDbName = ConfigurationManager.AppSettings["currectDb"];
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings[_currectDbName].ConnectionString;

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}