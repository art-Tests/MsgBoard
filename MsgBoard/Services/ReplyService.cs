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
where r.PostId =@id
order by r.CreateTime
";
        }

        public Reply GetReplyById(IDbConnection conn, int id)
        {
            var sqlCmd = "select top 1 * from [dbo].[Reply] (nolock) where id=@id";
            return conn.QueryFirstOrDefault<Reply>(sqlCmd, new { id });
        }
    }
}