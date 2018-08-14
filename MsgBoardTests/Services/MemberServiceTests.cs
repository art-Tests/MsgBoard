using System;
using System.Data;
using System.Data.SqlClient;
using ExpectedObjects;
using HashUtility.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Models.Interface;
using MsgBoard.Services;
using NSubstitute;

namespace MsgBoardTests.Services
{
    [TestClass()]
    public class MemberServiceTests
    {
        [TestMethod]
        public void CheckUserPasswordTest2()
        {
            var email = "art.huang@ehsn.com.tw";
            var userPass = "1234";
            var fakeUser = GetFakeUser(email);
            var expected = new UserLoginResult()
            {
                Auth = true,
                User = fakeUser
            };

            var fakeSqlConnection = new SqlConnection();
            var sut = new MemberServiceStub();
            var hashTool = new HashService();
            hashTool.SetAlgList("SHA512,SHA256");
            sut.SetHashTool(hashTool);
            var actual = sut.CheckUserPassword(fakeSqlConnection, email, userPass);
            expected.ToExpectedObject().ShouldEqual(actual);
        }

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

            var connFactory = Substitute.For<IConnectionFactory>();
            connFactory.GetConnection().Returns(new SqlConnection());
            var connection = connFactory.GetConnection();

            var sut = new MemberService();

            var userRepo = Substitute.For<IUserRepository>();
            userRepo.GetUserByMail(connection, fakeUser.Mail).Returns(fakeUser);
            sut.SetUserRepository(userRepo);

            var passRepo = Substitute.For<IPasswordRepository>();
            passRepo.FindPasswordByUserId(connection, fakeUser.Id).Returns(GetFakePassword());
            sut.SetPasswordRepository(passRepo);

            var hashTool = new HashService();
            hashTool.SetAlgList("SHA512,SHA256");
            sut.SetHashTool(hashTool);

            var actual = sut.CheckUserPassword(connection, email, userPass);
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        private static Password GetFakePassword()
        {
            var fakePassword = new Password
            {
                Id = 1,
                UserId = 1,
                HashPw = "11E1252247D8CB030F3DDC69BE23C2FE4AFD12C446AC6F229435B078A5451F2B",
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

    internal class MemberServiceStub : MemberService
    {
        public override UserLoginResult CheckUserPassword(IDbConnection connection, string email, string userPass)
        {
            var result = new UserLoginResult();

            var user = GetFakeUser(email);
            if (user == null) return result;

            var password = GetFakePassword();
            if (password == null) return result;

            var hashPassword = HashService.GetMemberHashPw(user.Guid, userPass);
            result.Auth = password.HashPw == hashPassword;
            result.User = user;
            return result;
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

        private static Password GetFakePassword()
        {
            var fakePassword = new Password
            {
                Id = 1,
                UserId = 1,
                HashPw = "11E1252247D8CB030F3DDC69BE23C2FE4AFD12C446AC6F229435B078A5451F2B",
                CreateTime = DateTime.Now
            };
            return fakePassword;
        }
    }
}