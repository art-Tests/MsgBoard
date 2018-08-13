using System.Security.Cryptography;

namespace HashUtility.Services
{
    public class HashAlgorithmFactory
    {
        public HashAlgorithm GetInstance(string hashType)
        {
            switch (hashType.ToUpper())
            {
                case "SHA256": return new SHA256Managed();
                case "SHA384": return new SHA384Managed();
                case "SHA512": return new SHA512Managed();
            }
            return null;
        }
    }
}