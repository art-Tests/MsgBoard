using HashUtility.Interface;
using HashUtility.Services.Hash;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashUtilityTests.Services
{
    [TestClass]
    public class Sha512HashToolTests
    {
        private IHash _sut;

        [TestInitialize]
        public void BeforeEach()
        {
            _sut = new Sha512HashTool();
        }

        [TestMethod]
        public void BaseHashTest()
        {
            var source = "123456";
            var expected = "BA3253876AED6BC22D4A6FF53D8406C6AD864195ED144AB5C87621B6C233B548BAEAE6956DF346EC8C17F5EA10F35EE3CBC514797ED7DDD3145464E2A0BAB413";

            var actual = _sut.GetHash(source);
            Assert.AreEqual(expected, actual);
        }
    }
}