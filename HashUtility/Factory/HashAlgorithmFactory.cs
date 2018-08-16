using System.Security.Cryptography;

namespace HashUtility.Factory
{
    public class HashAlgorithmFactory
    {
        public static HashAlgorithm GetInstance(string hashType)
        {
            if (string.IsNullOrEmpty(hashType)) return null;
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