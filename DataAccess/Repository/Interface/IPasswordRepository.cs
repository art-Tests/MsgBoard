using System.Collections.Generic;
using System.Data;
using DataModel.Entity;

namespace DataAccess.Repository.Interface
{
    public interface IPasswordRepository
    {
        /// <summary>
        /// 依據UserId取得Password Entity
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Password FindPasswordByUserId(IDbConnection conn, int userId);

        /// <summary>
        /// 取得歷史密碼資料
        /// </summary>
        /// <param name="conn">The conn.</param>
        /// <param name="userId">會員Id</param>
        /// <returns></returns>
        IEnumerable<Password> GetUserHistroyPasswords(IDbConnection conn, int userId);

        /// <summary>
        /// 新增密碼
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="entity">密碼entity</param>
        /// <returns></returns>
        bool Create(IDbConnection conn, Password entity);
    }
}