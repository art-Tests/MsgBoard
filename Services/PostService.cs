﻿using System.Data;
using System.Linq;
using System.Transactions;
using Dapper;
using DataAccess.Interface;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using DataModel.Entity;
using MsgBoard.DataModel.Dto;
using MsgBoard.DataModel.ViewModel.Post;

namespace Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepo = new PostRepository();
        private readonly IReplyRepository _replyRepo = new ReplyRepository();

        private readonly IConnectionFactory _connFactory;
        private readonly IDbConnection _conn;

        public PostService(IConnectionFactory factory)
        {
            _connFactory = factory;
            _conn = _connFactory.GetConnection();
        }

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        public Post GetPostById(int id)
        {
            return _postRepo.GetPostById(_conn, id);
        }

        /// <summary>
        /// 刪除文章及回覆
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeletePostAndReply(int id)
        {
            using (var transScope = new TransactionScope())
            {
                using (var connection = _connFactory.GetConnection())
                {
                    // Delete Post
                    _postRepo.Delete(connection, id);
                    // Delete Reply
                    _replyRepo.DeleteByPostId(connection, id);
                }
                transScope.Complete();
            }

            // 刪除文章、回復，有可能刪除到管理者或是其他人的資料，因此直接重新刷新目前User的文章數量資訊
            var artCnt = new UserArticleCount()
            {
                PostCount = _postRepo.GetPostCountByUserId(_conn, SignInUser.User.Id),
                ReplyCount = _replyRepo.GetReplyCountByUserId(_conn, SignInUser.User.Id)
            };
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
            _postRepo.Create(_conn, model);

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
            _postRepo.Update(_conn, dbPost);
        }

        /// <summary>
        /// 取得文章列表資料
        /// </summary>
        /// <param name="id">會員編號，有傳入表示查詢該會員的文章</param>
        /// <param name="queryItem">The query item.</param>
        /// <returns></returns>
        public IQueryable<PostIndexViewModel> GetPostCollection(int? id, string queryItem)
        {
            return GetPostCollection(_conn, id, queryItem);
        }

        /// <summary>
        /// 取得文章列表資料
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">會員編號，有傳入表示查詢該會員的文章</param>
        /// <param name="queryItem">The query item.</param>
        /// <returns></returns>
        private IQueryable<PostIndexViewModel> GetPostCollection(IDbConnection conn, int? id, string queryItem)
        {
            var sqlCmd = id == null
                ? GetPostCollectionSqlCmd()
                : queryItem.ToUpper() == "POST"
                    ? GetPostCollectionByUserSqlCmd()
                    : GetReplyCollectionByUserSqlCmd();

            return conn.Query<PostIndexViewModel, Author, Author, PostIndexViewModel>(sqlCmd, (p, i, u) =>
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
    }
}