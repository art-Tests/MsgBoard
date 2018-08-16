using System.Collections.Generic;
using System.Web.Http;
using MsgBoard.Models.ViewModel.Reply;
using MsgBoard.Services;

namespace MsgBoard.ApiControllers
{
    public class ReplyController : ApiController
    {
        private readonly ReplyService _replyService = new ReplyService();

        // GET: api/Reply/18
        public IEnumerable<ReplyIndexViewModel> Get(int id, int user = 0)
        {
            return _replyService.GetReplyByPostId(id, user);
        }
    }
}