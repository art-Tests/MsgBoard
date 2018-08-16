using System.Collections.Generic;
using System.Web.Http;
using MsgBoard.Models.ViewModel.Reply;
using MsgBoard.Services;
using MsgBoard.Services.Factory;
using MsgBoard.Services.Interface;

namespace MsgBoard.ApiControllers
{
    public class ReplyController : ApiController
    {
        private readonly IConnectionFactory _connFactory = new ConnectionFactory();

        private readonly ReplyService _replyService;

        public ReplyController()
        {
            _replyService = new ReplyService(_connFactory);
        }

        // GET: api/Reply/18
        public IEnumerable<ReplyIndexViewModel> Get(int id, int user = 0)
        {
            return _replyService.GetReplyByPostId(id, user);
        }
    }
}