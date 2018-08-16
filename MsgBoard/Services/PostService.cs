using System.Data;
using System.Linq;
using System.Transactions;
using DataAccess.Interface;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Models.ViewModel.Post;
using MsgBoard.Services.Interface;
using MsgBoard.Services.Repository;

namespace MsgBoard.Services
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
            return _postRepo.GetPostCollection(_conn, id, queryItem);
        }
    }
}