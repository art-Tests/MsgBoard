using System.Configuration;
using System.Web.Mvc;
using MsgBoard.Services;

namespace MsgBoard.Controllers
{
    public class BaseController : Controller
    {
        protected ConnectionFactory _connFactory;
        protected string FileUploadPath = ConfigurationManager.AppSettings["uploadPath"];

        public BaseController()
        {
            _connFactory = new ConnectionFactory();
        }
    }
}