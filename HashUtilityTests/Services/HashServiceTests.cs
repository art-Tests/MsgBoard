using HashUtility.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashUtilityTests.Services
{
    [TestClass()]
    public class HashServiceTests
    {
        private HashService _sut;

        [TestInitialize]
        public void BeforeEach()
        {
            _sut = new HashService();
            _sut.SetAlgList("SHA512,SHA256");
        }

        [TestMethod()]
        public void Base_Hash_Test()
        {
            var memberGuid = "";
            var memberPass = "123456";
            var expected = "ED680CDE9FE4488DEC7D330DF7EC7176AE23ECFAC0DB9EB68786227DF78F66AF";
            _sut.SetHashKey(string.Empty);

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Base_Guid_Hash_Test()
        {
            var memberGuid = "c4ef02b1-5dcc-469a-a38a-58b95aa62dee";
            var memberPass = "123456";
            var expected = "1F15BAF073BE36AFE66F8FAE8E897E1587E324F94562610FFBA5ABD688B5ED98";
            _sut.SetHashKey(string.Empty);

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Base_Guid_Key_Hash_Test()
        {
            var memberGuid = "94a02d6c-da5d-4a43-a03d-d370db007539";
            var memberPass = "123456";
            var expected = "38DE51D03073B79DA1E65638FBBB5E7867BEEAF1ACB90F225DC7EA34A5528669";
            _sut.SetHashKey("i_am_security_key");

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Base_Guid_Key_Hash_Test_2()
        {
            var memberGuid = "b2cba1ae-44fb-41a7-830b-0bb8eeab3dd8";
            var memberPass = "1234";
            var expected = "6D567DAFBA3768A740DE3C2B5F8F745BFD71BFD36234E4F1A9A6BED17A903EFF";
            _sut.SetHashKey("i_am_security_key");

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }
    }
}