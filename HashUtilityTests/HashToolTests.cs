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

        [TestMethod()]
        public void GetMemberHashPwTest2()
        {
            var memberGuid = "c4ef02b1-5dcc-469a-a38a-58b95aa62dee";
            var memberPass = "1234";
            var expected = "thKsYztx8mXq9IWWnozBpzWQpwtHBTTpOa4ya9PgQe3K4j3he8KCBIoxoDbRtT6vn4ln9SGLAa6vugUwznAm3Q==";
            _sut.SetHashKey(string.Empty);

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMemberHashPwTest3()
        {
            var memberGuid = "94a02d6c-da5d-4a43-a03d-d370db007539";
            var memberPass = "1234";
            var expected = "YTW05bMV4+AVJ1tFVDY24Td24Qyi1Qd7iE2GUnPYm+pFcDM8gEn8PixvqgeCNlVGzdvG3g30qJcouMIS/D2xUA==";

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }
    }
}