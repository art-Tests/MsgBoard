using System.Collections.Generic;
using System.Data;
using Dapper;
using DataAccess.Interface;
using MsgBoard.DataModel.ViewModel.Admin;

namespace Services
{
    public class AdminService
    {
        private readonly IDbConnection _conn;

        public AdminService(IConnectionFactory factory)
        {
            _conn = factory.GetConnection();
        }

        /// <summary>
        /// 取得所有會員資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdminIndexViewModel> GetUserCollection()
        {
            var sqlCmd = GetAllUserSqlCmd();
            return _conn.Query<AdminIndexViewModel>(sqlCmd);
        }

        private string GetAllUserSqlCmd()
        {
            return @"
	select ROW_NUMBER() OVER(ORDER BY Id) AS RowId, Id, Pic, Name, Mail, IsAdmin, IsDel
    from [dbo].[user] (nolock)";
        }
    }
}