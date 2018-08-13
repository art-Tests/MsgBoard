﻿namespace HashUtility.Services
{
    /// <summary>
    /// SHA512雜湊
    /// </summary>
    public class Sha512HashTool : BaseHashTool
    {
        public Sha512HashTool()
        {
            HashType = "SHA512";
        }
    }
}