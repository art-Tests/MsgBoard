using System.Collections.Generic;
using System.Data;
using Dapper;
using DataAccess.Interface;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using DataModel.Entity;
using MsgBoard.DataModel.Dto;
using MsgBoard.DataModel.ViewModel.Reply;

namespace Services
{
    public class ReplyService
    {
        private readonly IReplyRepository _replyRepo = new ReplyRepository();
        private readonly IPostRepository _postRepo = new PostRepository();
        private readonly IDbConnection _conn;

        public ReplyService(IConnectionFactory factory)
        {
            _conn = factory.GetConnection();
        }

        /// <summary>
        /// 新增回覆
        /// </summary>
        /// <param name="model">回覆entity</param>
        /// <returns></returns>
        public int CreateReply(Reply model)
        {
            model.CreateUserId = SignInUser.User.Id;
            model.UpdateUserId = SignInUser.User.Id;
            var id = _replyRepo.Create(_conn, model);
            SignInUser.AdjustReplyCnt(1);
            return id;
        }

        /// <summary>
        /// 取得文章的所有回覆
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        public IEnumerable<ReplyIndexViewModel> GetReplyByPostId(int id, int userId)
        {
            return GetReplyByPostId(_conn, id, userId);
        }

        /// <summary>
        /// 取得文章的所有回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章Id</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        public IEnumerable<ReplyIndexViewModel> GetReplyByPostId(IDbConnection conn, int id, int userId)
        {
            var sqlCmd = GetReplyByPostIdSqlCmd();
            return conn.Query<ReplyIndexViewModel, Author, Author, ReplyIndexViewModel>(sqlCmd, (r, i, u) =>
            {
                i.Pic = i.Pic.Replace("~", string.Empty);
                u.Pic = u.Pic.Replace("~", string.Empty);
                r.CreateAuthor = i;
                r.UpdateAuthor = u;
                return r;
            }, new { id, userId });
        }

        private string GetReplyByPostIdSqlCmd()
        {
            return @"
select r.*
,(select IsAdmin from [dbo].[User] (nolock) where Id=@userId) as IsAdmin
,(select @userId )as UserId
,insUser.*
,updUser.*
from [dbo].[Reply] (nolock) r
left join [dbo].[User] (nolock) insUser on insUser.Id = r.CreateUserId
left join [dbo].[User] (nolock) updUser on updUser.Id = r.UpdateUserId
where r.PostId =@id and r.IsDel=0
order by r.CreateTime
";
        }

        /// <summary>
        /// 依據回覆Id取得Entity
        /// </summary>
        /// <param name="id">回覆Id</param>
        /// <returns></returns>
        public Reply GetReplyById(int id) => _replyRepo.GetReplyById(_conn, id);

        /// <summary>
        /// 刪除某筆回覆
        /// </summary>
        /// <param name="model">要刪除的回覆entity</param>
        public void DeleteReply(Reply model)
        {
            _replyRepo.Delete(_conn, model.Id);
            if (model.CreateUserId == SignInUser.User.Id)
            {
                SignInUser.AdjustReplyCnt(-1);
            }
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="reply">The reply.</param>
        public void UpdateReply(Reply model, Reply reply)
        {
            reply.Content = model.Content;
            reply.UpdateUserId = SignInUser.User.Id;
            _replyRepo.Update(_conn, reply);
        }

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="id">文章編號</param>
        /// <returns></returns>
        public Post GetPostById(int id) => _postRepo.GetPostById(_conn, id);
    }
}