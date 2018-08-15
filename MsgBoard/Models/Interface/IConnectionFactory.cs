using System.Data;

namespace MsgBoard.Models.Interface
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}