using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Policy;
using System.Web.Mvc;
using Dapper;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.ViewModel.Reply;

namespace MsgBoard.Services
{
    public class ReplyService
    {
        /// <summary>
        /// 新增回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="model">回覆entity</param>
        /// <returns></returns>
        public int Create(IDbConnection conn, Reply model)
        {
            var sqlCmd = GetCreateSqlCmd();
            return conn.QueryFirstOrDefault<int>(sqlCmd, model);
        }

        private string GetCreateSqlCmd()
        {
            return @"
INSERT INTO [dbo].[Reply]
           ([PostId]
           ,[Content]
           ,[IsDel]
           ,[CreateUserId]
           ,[UpdateUserId])
     VALUES
           (@PostId
           ,@Content
           ,0
           ,@CreateUserId
           ,@UpdateUserId)

SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]
";
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
            }, new { id, userId }, splitOn: "Id");
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
        /// <param name="conn">The connection.</param>
        /// <param name="id">回覆Id</param>
        /// <returns></returns>
        public Reply GetReplyById(IDbConnection conn, int id)
        {
            var sqlCmd = "select top 1 * from [dbo].[Reply] (nolock) where id=@id";
            return conn.QueryFirstOrDefault<Reply>(sqlCmd, new { id });
        }

        /// <summary>
        /// 更新回覆Entity
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="reply">回覆Entity</param>
        public void Update(IDbConnection conn, Reply reply)
        {
            var sqlCmd = GetUpdateSqlCmd();
            conn.Execute(sqlCmd, reply);
        }

        private string GetUpdateSqlCmd()
        {
            return @"
UPDATE [dbo].[Reply]
   SET [Content] = @Content
      ,[UpdateTime] = GetDate()
      ,[UpdateUserId] = @UpdateUserId
 WHERE Id=@id
";
        }

        /// <summary>
        /// 刪除某筆回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">回覆Id</param>
        public void Delete(IDbConnection conn, int id)
        {
            var sqlCmd = "Update [dbo].[Reply] set IsDel=1 where Id=@id";
            conn.Execute(sqlCmd, new { id });
        }

        /// <summary>
        /// 刪除某文章的所有回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章Id</param>
        public void DeleteByPostId(IDbConnection conn, int id)
        {
            var sqlCmd = "Update [dbo].[Reply] set IsDel=1 where PostId=@id";
            conn.Execute(sqlCmd, new { id });
        }
    }
}