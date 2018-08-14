using System.Configuration;
using System.Data;
using System.Web.Http;
using MsgBoard.Services;

namespace MsgBoard.ApiControllers
{
    public class BaseApiController : ApiController
    {
        protected ConnectionFactory ConnFactory;
        protected string FileUploadPath = ConfigurationManager.AppSettings["uploadPath"];
        protected IDbConnection Conn;

        public BaseApiController()
        {
            ConnFactory = new ConnectionFactory();
            Conn = ConnFactory.GetConnection();
        }
    }
}