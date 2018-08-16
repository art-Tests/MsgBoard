using HashUtility.Factory;
using HashUtility.Services.Hash;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HashUtilityTests.Services
{
    [TestClass()]
    public class HashFactoryTests
    {
        [TestMethod()]
        public void Get_SHA256_Instance_Test()
        {
            var target = "SHA256";
            var expected = new Sha256HashTool().HashType;

            var actual = HashFactory.GetInstance(target).HashType;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Get_SHA512_Instance_Test()
        {
            var target = "SHA512";
            var expected = new Sha512HashTool().HashType;

            var actual = HashFactory.GetInstance(target).HashType;
            Assert.AreEqual(expected, actual);
        }
    }
}