using System.Data;
using DataModel.Entity;

namespace DataAccess.Repository.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// 依據會員Email取得User Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        User GetUserByMail(IDbConnection connection, string email);

        /// <summary>
        /// 依據會員Id取得User Entity
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="id">會員Id</param>
        /// <returns></returns>
        User GetUserById(IDbConnection connection, int id);

        /// <summary>
        /// 會員資料修改
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="user">會員資料entity</param>
        void Update(IDbConnection connection, User user);

        /// <summary>
        /// 檢查系統是否已經存在相同使用者帳號
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="email">會員Email</param>
        /// <returns></returns>
        bool CheckUserExist(IDbConnection conn, string email);

        /// <summary>
        /// 新增會員
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="entity">會員entity</param>
        /// <returns></returns>
        int Create(IDbConnection conn, User entity);
    }
}