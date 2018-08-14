using HashUtility.Interface;

namespace HashUtility.Services
{
    public class HashFactory
    {
        public static IHash GetInstance(string hashType)
        {
            if (string.IsNullOrEmpty(hashType)) return null;
            switch (hashType.ToUpper())
            {
                case "SHA256": return new Sha256HashTool();
                case "SHA512": return new Sha512HashTool();
            }
            return null;
        }
    }
}