using System.Data;
using System.Linq;
using Dapper;
using MsgBoard.Models.Entity;
using MsgBoard.ViewModel.Post;

namespace MsgBoard.Services
{
    public class PostService
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

        /// <summary>
        /// 取得文章列表資料
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        public IQueryable<PostIndexViewModel> GetPostCollection(IDbConnection conn)
        {
            var sqlCmd = GetPostCollectionSqlCmd();
            return conn.Query<PostIndexViewModel, User, PostIndexViewModel>(sqlCmd, (p, u) =>
            {
                p.Author = u;
                return p;
            }).AsQueryable();
        }

        private string GetPostCollectionSqlCmd()
        {
            return @"
select ROW_NUMBER() OVER(ORDER BY p.Id desc) AS RowId, p.*
,u.Id,u.Name,u.Pic
from [dbo].[Post] (nolock) as p
left join [dbo].[User] (nolock) as u on p.UpdateUserId= u.Id
where p.IsDel=0
";
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
        /// 修改文章資料
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="post">文章entity</param>
        public void Update(IDbConnection conn, Post post)
        {
            var sqlCmd = GetUpdateSqlCmd();
            conn.Execute(sqlCmd, post);
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
    }
}