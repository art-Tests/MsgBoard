using System.Collections.Generic;
using MsgBoard.Services;
using MsgBoard.ViewModel.Reply;

namespace MsgBoard.ApiControllers
{
    public class ReplyController : BaseApiController
    {
        private readonly ReplyService _replyService = new ReplyService();

        // GET: api/Reply/18
        public IEnumerable<ReplyIndexViewModel> Get(int id, int user = 0)
        {
            return _replyService.GetReplyByPostId(Conn, id, user);
        }
    }
}