using System.Data;
using System.Linq;
using MsgBoard.Models.Entity;
using MsgBoard.ViewModel.Post;

namespace MsgBoard.Services.Interface
{
    public interface IPostRepository
    {
        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="entity">文章entity</param>
        /// <returns>回傳文章編號</returns>
        int Create(IDbConnection conn, Post entity);

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        Post GetPostById(IDbConnection conn, int id);

        /// <summary>
        /// 將文章設為刪除
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章編號</param>
        void Delete(IDbConnection conn, int id);

        /// <summary>
        /// 修改文章資料
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="post">文章entity</param>
        void Update(IDbConnection conn, Post post);

        /// <summary>
        /// 取得文章列表資料
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">會員編號，有傳入表示查詢該會員的文章</param>
        /// <param name="queryItem">The query item.</param>
        /// <returns></returns>
        IQueryable<PostIndexViewModel> GetPostCollection(IDbConnection conn, int? id, string queryItem);

        /// <summary>
        /// 取得會員發文數量
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">會員編號</param>
        /// <returns></returns>
        int GetPostCountByUserId(IDbConnection conn, int id);
    }
}