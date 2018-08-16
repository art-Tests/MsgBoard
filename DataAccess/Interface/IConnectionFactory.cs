using System.Data;

namespace DataAccess.Interface
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}