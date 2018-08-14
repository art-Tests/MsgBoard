using System.Configuration;
using System.Data;
using System.Web.Mvc;
using MsgBoard.Services;

namespace MsgBoard.Controllers
{
    public class BaseController : Controller
    {
        protected ConnectionFactory ConnFactory;
        protected string FileUploadPath = ConfigurationManager.AppSettings["uploadPath"];
        protected IDbConnection Conn;

        public BaseController()
        {
            ConnFactory = new ConnectionFactory();
            Conn = ConnFactory.GetConnection();
        }
    }
}