using HashUtility;
using HashUtility.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashUtilityTests
{
    [TestClass()]
    public class HashToolTests
    {
        private HashTool _sut;

        [TestInitialize]
        public void BeforeEach()
        {
            _sut = new HashTool();
        }

        [TestMethod()]
        public void GetMemberHashPwTest()
        {
            var memberGuid = "";
            var memberPass = "abcdefg";
            var expected = "1xakGIVptoqxtt+sF45XARTN8Oo6HMDjFIbD5BJBvGp2Qk6MN6sm8Jb8he+YhsjLY0GH9P3f9kX7CZ8f9UxrjA==";
            _sut.SetHashKey(string.Empty);

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }
    }
}