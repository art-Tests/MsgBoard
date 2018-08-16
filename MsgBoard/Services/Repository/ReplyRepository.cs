using System.Data;
using Dapper;

namespace MsgBoard.Services.Repository
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
    }

    public interface IReplyRepository
    {
        /// <summary>
        /// 刪除某文章的所有回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章Id</param>
        void DeleteByPostId(IDbConnection conn, int id);

        /// <summary>
        /// 取得會員回覆數量
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">會員編號</param>
        /// <returns></returns>
        int GetReplyCountByUserId(IDbConnection conn, int id);
    }
}