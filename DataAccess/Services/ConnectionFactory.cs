using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DataAccess.Interface;

namespace DataAccess.Services
{
    public class ConnectionFactory : IConnectionFactory
    {
        public string ConnectionString { get; set; }

        public ConnectionFactory()
        {
            var currectDbName = Properties.Settings.Default.currectDb;
            ConnectionString = ConfigurationManager.ConnectionStrings[currectDbName].ConnectionString;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}