using System.Collections.Generic;
using System.Data;
using DataAccess.Interface;
using DataAccess.Repository;
using DataAccess.Repository.Interface;
using DataModel.Entity;

namespace Services
{
    public class AdminService
    {
        private readonly IUserRepository _userRepo = new UserRepository();

        private readonly IDbConnection _conn;

        public AdminService(IConnectionFactory factory)
        {
            _conn = factory.GetConnection();
        }

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUserCollection()
        {
            return _userRepo.GetAllUser(_conn);
        }
    }
}