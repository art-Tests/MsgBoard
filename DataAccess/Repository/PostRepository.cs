using System.Data;
using Dapper;
using DataAccess.Repository.Interface;
using DataModel.Entity;

namespace DataAccess.Repository
{
    public class PostRepository : IPostRepository
    {
        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="entity">文章entity</param>
        /// <returns>回傳文章編號</returns>
        public int Create(IDbConnection conn, Post entity)
        {
            var sqlCmd = GetCreateSqlCmd();
            return conn.QueryFirstOrDefault<int>(sqlCmd, entity);
        }

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        public Post GetPostById(IDbConnection conn, int id)
        {
            var sqlCmd = "select top 1 * from [dbo].[Post] (nolock) where Id=@id";
            return conn.QueryFirstOrDefault<Post>(sqlCmd, new { id });
        }

        /// <summary>
        /// 將文章設為刪除
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章編號</param>
        public void Delete(IDbConnection conn, int id)
        {
            var sqlCmd = "update [dbo].[Post] set [IsDel] = 1 where Id=@id";
            conn.Execute(sqlCmd, new { id });
        }

        /// <summary>
        /// 修改文章資料
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="post">文章entity</param>
        public void Update(IDbConnection conn, Post post)
        {
            var sqlCmd = GetUpdateSqlCmd();
            conn.Execute(sqlCmd, post);
        }

        /// <summary>
        /// 取得會員發文數量
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">會員編號</param>
        /// <returns></returns>
        public int GetPostCountByUserId(IDbConnection conn, int id)
        {
            var sqlCmd =
                "select count(*) as [PostCount] from [dbo].[Post] (nolock) where CreateUserId = @Id and IsDel=0";
            return conn.QueryFirstOrDefault<int>(sqlCmd, new { id });
        }

        private string GetUpdateSqlCmd()
        {
            return @"
UPDATE [dbo].[Post]
   SET [Content] = @Content
      ,[UpdateTime] = GetDate()
      ,[UpdateUserId] = @UpdateUserId
 WHERE Id=@id
";
        }

        private string GetCreateSqlCmd()
        {
            return @"
INSERT INTO [dbo].[Post]
           ([Content]
           ,[IsDel]
           ,[CreateUserId]
           ,[UpdateUserId])
     VALUES
           (@Content
           ,@IsDel
           ,@CreateUserId
           ,@UpdateUserId)

SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]
";
        }
    }
}