using System.Data;
using System.Linq;
using System.Transactions;
using Dapper;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Services.Common;
using MsgBoard.ViewModel.Post;

namespace MsgBoard.Services
{
    public class PostService : BaseService
    {
        private readonly ReplyService _replyService = new ReplyService();
        private readonly MemberService _memberService = new MemberService();

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
        /// <param name="id">會員編號，有傳入表示查詢該會員的文章</param>
        /// <param name="queryItem">The query item.</param>
        /// <returns></returns>
        public IQueryable<PostIndexViewModel> GetPostCollection(int? id, string queryItem)
        {
            var sqlCmd = id == null
                ? GetPostCollectionSqlCmd()
                : queryItem.ToUpper() == "POST"
                    ? GetPostCollectionByUserSqlCmd()
                    : GetReplyCollectionByUserSqlCmd();

            return Conn.Query<PostIndexViewModel, Author, Author, PostIndexViewModel>(sqlCmd, (p, i, u) =>
             {
                 p.CreateAuthor = i;
                 p.UpdateAuthor = u;
                 return p;
             }, new { id }).AsQueryable();
        }

        /// <summary>
        /// 取得使用者已回覆的文章SQL (包含管理者修改他人之文章)
        /// </summary>
        /// <returns></returns>
        private string GetReplyCollectionByUserSqlCmd()
        {
            return @"
select ROW_NUMBER() OVER(ORDER BY p.Id desc) AS RowId, p.*
,(
	select count(*) from reply where PostId = p.Id and IsDel=0
) as 'ReplyCount'
,i.*
,u.*
from [dbo].[Post] (nolock) as p
left join [dbo].[User] (nolock) as i on p.CreateUserId= i.Id
left join [dbo].[User] (nolock) as u on p.UpdateUserId= u.Id
where p.IsDel=0
and p.Id in (select PostId from [dbo].[Reply] r where ( r.CreateUserId = @id or r.UpdateUserId = @id) and r.IsDel=0)
";
        }

        /// <summary>
        /// 取得使用者發佈的文章SQL
        /// </summary>
        /// <returns></returns>
        private string GetPostCollectionByUserSqlCmd()
        {
            return @"
select ROW_NUMBER() OVER(ORDER BY p.Id desc) AS RowId, p.*
,(
	select count(*) from reply where PostId = p.Id and IsDel=0
) as 'ReplyCount'
,i.*
,u.*
from [dbo].[Post] (nolock) as p
left join [dbo].[User] (nolock) as i on p.CreateUserId= i.Id
left join [dbo].[User] (nolock) as u on p.UpdateUserId= u.Id
where p.IsDel=0
and p.CreateUserId=@id
";
        }

        /// <summary>
        /// 取得所有文章SQL
        /// </summary>
        /// <returns></returns>
        private string GetPostCollectionSqlCmd()
        {
            return @"
select ROW_NUMBER() OVER(ORDER BY p.Id desc) AS RowId, p.*
,(
	select count(*) from reply where PostId = p.Id and IsDel=0
) as 'ReplyCount'
,i.*
,u.*
from [dbo].[Post] (nolock) as p
left join [dbo].[User] (nolock) as i on p.CreateUserId= i.Id
left join [dbo].[User] (nolock) as u on p.UpdateUserId= u.Id
where p.IsDel=0
";
        }

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        public Post GetPostById(int id)
        {
            var sqlCmd = "select top 1 * from [dbo].[Post] (nolock) where Id=@id";
            return Conn.QueryFirstOrDefault<Post>(sqlCmd, new { id });
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

        /// <summary>
        /// 刪除文章及回覆
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeletePostAndReply(int id)
        {
            using (var transScope = new TransactionScope())
            {
                using (var connection = ConnFactory.GetConnection())
                {
                    // Delete Post
                    Delete(connection, id);
                    // Delete Reply
                    _replyService.DeleteByPostId(connection, id);
                }
                transScope.Complete();
            }

            // 刪除文章、回復，有可能刪除到管理者或是其他人的資料，因此直接重新刷新目前User的文章數量資訊
            var artCnt = _memberService.GetUserArticleCount(Conn, SignInUser.User.Id);
            SignInUser.SetArticleCount(artCnt);
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="model">The model.</param>
        public void CreatePost(Post model)
        {
            model.CreateUserId = SignInUser.User.Id;
            model.UpdateUserId = SignInUser.User.Id;
            Create(Conn, model);

            SignInUser.AdjustPostCnt(1);
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="dbPost">The database post.</param>
        public void UpdatePost(Post model, Post dbPost)
        {
            dbPost.Content = model.Content;
            dbPost.UpdateUserId = SignInUser.User.Id;
            Update(Conn, dbPost);
        }
    }
}