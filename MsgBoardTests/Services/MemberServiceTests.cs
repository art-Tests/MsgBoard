using System;
using System.Data.SqlClient;
using DataAccess.Interface;
using DataAccess.Repository.Interface;
using DataModel.Entity;
using ExpectedObjects;
using HashUtility.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsgBoard.DataModel.Dto;
using MsgBoard.Services;
using NSubstitute;

namespace MsgBoardTests.Services
{
    [TestClass()]
    public class MemberServiceTests
    {
        [TestMethod()]
        public void CheckUserPasswordTest()
        {
            var email = "art.huang@ehsn.com.tw";
            var userPass = "1234";
            var fakeUser = GetFakeUser(email);
            var expected = new UserLoginResult()
            {
                Auth = true,
                User = fakeUser
            };

            var fakeConn = new SqlConnection();
            var connFactory = Substitute.For<IConnectionFactory>();
            connFactory.GetConnection().Returns(fakeConn);

            var sut = new MemberService(connFactory);

            var userRepo = Substitute.For<IUserRepository>();
            userRepo.GetUserByMail(fakeConn, fakeUser.Mail).Returns(fakeUser);
            sut.SetUserRepository(userRepo);

            var passRepo = Substitute.For<IPasswordRepository>();
            passRepo.FindPasswordByUserId(fakeConn, fakeUser.Id).Returns(GetFakePassword());
            sut.SetPasswordRepository(passRepo);

            var hashTool = new HashService();
            hashTool.SetAlgList("SHA512,SHA256");
            hashTool.SetHashKey("i_am_security_key");
            sut.SetHashTool(hashTool);

            var actual = sut.CheckUserPassword(email, userPass);
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        private static Password GetFakePassword()
        {
            var fakePassword = new Password
            {
                Id = 1,
                UserId = 1,
                HashPw = "6D567DAFBA3768A740DE3C2B5F8F745BFD71BFD36234E4F1A9A6BED17A903EFF",
                CreateTime = DateTime.Now
            };
            return fakePassword;
        }

        private static User GetFakeUser(string email)
        {
            var fakeUser = new User
            {
                Guid = "b2cba1ae-44fb-41a7-830b-0bb8eeab3dd8",
                Id = 1,
                IsAdmin = false,
                Mail = email,
                Name = "art",
                Pic = string.Empty
            };
            return fakeUser;
        }
    }
}