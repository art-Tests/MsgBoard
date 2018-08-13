using System.Data;
using System.Linq;
using Dapper;
using MsgBoard.Models.Entity;
using MsgBoard.ViewModel.Post;

namespace MsgBoard.Services
{
    public class PostService
    {
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
select ROW_NUMBER() OVER(ORDER BY p.Id) AS RowId, p.*
,u.Id,u.Name,u.Pic
from [dbo].[Post] (nolock) as p
left join [dbo].[User] (nolock) as u on p.UpdateUserId= u.Id
where p.IsDel=0
";
        }
    }
}