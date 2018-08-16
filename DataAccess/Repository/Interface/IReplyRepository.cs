using System.Collections.Generic;
using System.Data;
using DataModel.Entity;
using MsgBoard.DataModel.ViewModel.Reply;

namespace DataAccess.Repository.Interface
{
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

        /// <summary>
        /// 新增回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="model">回覆entity</param>
        /// <returns></returns>
        int Create(IDbConnection conn, Reply model);

        /// <summary>
        /// 取得回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Reply GetReplyById(IDbConnection conn, int id);

        /// <summary>
        /// 更新回覆Entity
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="reply">回覆Entity</param>
        void Update(IDbConnection conn, Reply reply);

        /// <summary>
        /// 刪除某筆回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">回覆Id</param>
        void Delete(IDbConnection conn, int id);

        /// <summary>
        /// 取得文章的所有回覆
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="id">文章Id</param>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        IEnumerable<ReplyIndexViewModel> GetReplyByPostId(IDbConnection conn, int id, int userId);
    }
}