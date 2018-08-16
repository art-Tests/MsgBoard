using System.Data;

namespace MsgBoard.Services.Interface
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}