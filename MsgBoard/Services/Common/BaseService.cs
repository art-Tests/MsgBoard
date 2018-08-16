using System.Configuration;
using System.Data;

namespace MsgBoard.Services.Common
{
    public class BaseService
    {
        protected ConnectionFactory ConnFactory;
        protected string FileUploadPath = ConfigurationManager.AppSettings["uploadPath"];
        protected IDbConnection Conn;

        public BaseService()
        {
            ConnFactory = new ConnectionFactory();
            Conn = ConnFactory.GetConnection();
        }
    }
}