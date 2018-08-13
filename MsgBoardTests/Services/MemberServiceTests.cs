using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.SqlClient;
using ExpectedObjects;
using MsgBoard.Models.Dto;
using MsgBoard.Models.Entity;
using MsgBoard.Models.Interface;
using NSubstitute;

namespace MsgBoard.Services.Tests
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

            var fakeSqlConnection = new SqlConnection();
            var connFactory = Substitute.For<IConnectionFactory>();
            connFactory.GetConnection().Returns(fakeSqlConnection);

            var connection = connFactory.GetConnection();
            var userRepo = Substitute.For<IUserRepository>();
            userRepo.GetUserByMail(connection, fakeUser.Mail).Returns(fakeUser);

            var passRepo = Substitute.For<IPasswordRepository>();
            passRepo.FindPasswordByUserId(connection, fakeUser.Id).Returns(GetFakePassword());

            var sut = new MemberService();
            sut.SetUserRepository(userRepo);
            sut.SetPasswordRepository(passRepo);

            var actual = sut.CheckUserPassword(connection, email, userPass);
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        private static Password GetFakePassword()
        {
            var fakePassword = new Password
            {
                Id = 1,
                UserId = 1,
                HashPw = "3041DF81726E8B5B3D1CACCF9FD6F5C7D8406B04D567CB00BD3E97711F71A0D9A7E4751A6416E83215F360781DE6DA6D4B6166F917D8668BB33DD0DD0B6554AA",
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

            var hashPassword = HashTool.GetMemberHashPw(user.Guid, userPass);
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
                HashPw = "3041DF81726E8B5B3D1CACCF9FD6F5C7D8406B04D567CB00BD3E97711F71A0D9A7E4751A6416E83215F360781DE6DA6D4B6166F917D8668BB33DD0DD0B6554AA",
                CreateTime = DateTime.Now
            };
            return fakePassword;
        }
    }
}