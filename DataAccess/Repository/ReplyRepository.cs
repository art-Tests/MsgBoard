using System.Data;
using Dapper;
using DataAccess.Repository.Interface;
using DataModel.Entity;

namespace DataAccess.Repository
{
    public class ReplyRepository : IReplyRepository
    {
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

        /// <summary>
        /// 取得會員回覆數量
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">會員編號</param>
        /// <returns></returns>
        public int GetReplyCountByUserId(IDbConnection conn, int id)
        {
            var sqlCmd =
                "select count(*) as [ReplyCount] from [dbo].[Reply] (nolock) where CreateUserId = @Id and IsDel=0";
            return conn.QueryFirstOrDefault<int>(sqlCmd, new { id });
        }

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

        /// <summary>
        /// 取得回覆
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
    }
}