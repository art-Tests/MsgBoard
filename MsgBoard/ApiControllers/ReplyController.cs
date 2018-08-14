using System.Collections.Generic;
using MsgBoard.Services;
using MsgBoard.ViewModel.Reply;

namespace MsgBoard.ApiControllers
{
    public class ReplyController : BaseApiController
    {
        private readonly ReplyService _replyService = new ReplyService();

        // GET: api/Reply/18
        public IEnumerable<ReplyIndexViewModel> Get(int id)
        {
            return _replyService.GetReplyByPostId(Conn, id);
        }
    }
}