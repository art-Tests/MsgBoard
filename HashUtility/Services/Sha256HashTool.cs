namespace HashUtility.Services
{
    /// <summary>
    /// SHA512雜湊
    /// </summary>
    public class Sha256HashTool : BaseHashTool
    {
        public Sha256HashTool()
        {
            HashType = "SHA256";
        }
    }
}