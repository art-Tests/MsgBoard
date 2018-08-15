using HashUtility.Interface;
using HashUtility.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashUtilityTests
{
    [TestClass]
    public class SHA256HashToolTests
    {
        private IHash _sut;

        [TestInitialize]
        public void BeforeEach()
        {
            _sut = new Sha256HashTool();
        }

        [TestMethod]
        public void BaseHashTest()
        {
            var source = "123456";
            var expected = "8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92";

            var actual = _sut.GetHash(source);
            Assert.AreEqual(expected, actual);
        }
    }
}