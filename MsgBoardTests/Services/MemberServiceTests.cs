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
            userRepo.FindUserByMail(connection, fakeUser.Mail).Returns(fakeUser);

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
                HashPw = "YTW05bMV4+AVJ1tFVDY24Td24Qyi1Qd7iE2GUnPYm+pFcDM8gEn8PixvqgeCNlVGzdvG3g30qJcouMIS/D2xUA==",
                CreateTime = DateTime.Now
            };
            return fakePassword;
        }

        private static User GetFakeUser(string email)
        {
            var fakeUser = new User
            {
                Guid = "94a02d6c-da5d-4a43-a03d-d370db007539",
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

            var hashPassword = _hashTool.GetMemberHashPw(user.Guid, userPass);
            result.Auth = password.HashPw == hashPassword;
            result.User = user;
            return result;
        }

        private static User GetFakeUser(string email)
        {
            var fakeUser = new User
            {
                Guid = "94a02d6c-da5d-4a43-a03d-d370db007539",
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
                HashPw = "YTW05bMV4+AVJ1tFVDY24Td24Qyi1Qd7iE2GUnPYm+pFcDM8gEn8PixvqgeCNlVGzdvG3g30qJcouMIS/D2xUA==",
                CreateTime = DateTime.Now
            };
            return fakePassword;
        }
    }
}