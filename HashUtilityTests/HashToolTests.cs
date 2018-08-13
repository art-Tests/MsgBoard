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
            _sut = new HashTool(HashFactory.GetInstance("SHA512"));
        }

        [TestMethod()]
        public void GetMemberHashPwTest()
        {
            var memberGuid = "";
            var memberPass = "123456";
            var expected = "BA3253876AED6BC22D4A6FF53D8406C6AD864195ED144AB5C87621B6C233B548BAEAE6956DF346EC8C17F5EA10F35EE3CBC514797ED7DDD3145464E2A0BAB413";
            _sut.SetHashKey(string.Empty);

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMemberHashPwTest2()
        {
            var memberGuid = "c4ef02b1-5dcc-469a-a38a-58b95aa62dee";
            var memberPass = "123456";
            var expected = "4F1ED0AC9D483AFB9C2B1AF0EA31DF154A007B3223944CFC02A62FAE1ABE2B1681818B977BB25042A155F867B4FCAF44921CF98605BB2633CCA20D7DDA5777C4";
            _sut.SetHashKey(string.Empty);

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMemberHashPwTest3()
        {
            var memberGuid = "94a02d6c-da5d-4a43-a03d-d370db007539";
            var memberPass = "123456";
            var expected = "96EC185FC2BD745E648C69BFB954638FA943027636FB70FFF0831480A9E010649205678B90DAE48F51D774505766D1F6A4352DDFD7343C83C3E8D9ABF75E3E19";

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetMemberHashPwTest4()
        {
            var memberGuid = "b2cba1ae-44fb-41a7-830b-0bb8eeab3dd8";
            var memberPass = "1234";
            var expected = "3041DF81726E8B5B3D1CACCF9FD6F5C7D8406B04D567CB00BD3E97711F71A0D9A7E4751A6416E83215F360781DE6DA6D4B6166F917D8668BB33DD0DD0B6554AA";

            var actual = _sut.GetMemberHashPw(memberGuid, memberPass);
            Assert.AreEqual(expected, actual);
        }
    }
}