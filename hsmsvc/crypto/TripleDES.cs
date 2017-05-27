using HSMSvc.Common;
using System;
using System.Security.Cryptography;

namespace HSMSvc.Crypto
{
    /// <summary>
    /// TripleDES算法加解密类
    /// </summary>
    static class TripleDES
    {
        public static string TripleDES_ECB_Encrypt(string key, string iv, string data)
        {
            return TripleDES_ECB_Encrypt(key.HexToByteArray(), iv.HexToByteArray(), data.HexToByteArray()).ByteArrayToHex();
        }
        public static byte[] TripleDES_ECB_Encrypt(byte[] key, byte[] iv, byte[] data)
        {
            return TripleDES_Encrypt(key, iv, data, CipherMode.ECB);
        }
        public static string TripleDES_CBC_Encrypt(string key, string iv, string data)
        {
            return TripleDES_CBC_Encrypt(key.HexToByteArray(), iv.HexToByteArray(), data.HexToByteArray()).ByteArrayToHex();
        }
        public static byte[] TripleDES_CBC_Encrypt(byte[] key, byte[] iv, byte[] data)
        {
            return TripleDES_Encrypt(key, iv, data, CipherMode.CBC);
        }
        public static string TripleDES_ECB_Decrypt(string key, string iv, string data)
        {
            return TripleDES_ECB_Decrypt(key.HexToByteArray(), iv.HexToByteArray(), data.HexToByteArray()).ByteArrayToHex();
        }
        public static byte[] TripleDES_ECB_Decrypt(byte[] key, byte[] iv, byte[] data)
        {
            return TripleDES_Decrypt(key, iv, data, CipherMode.ECB);
        }
        public static string TripleDES_CBC_Decrypt(string key, string iv, string data)
        {
            return TripleDES_CBC_Decrypt(key.HexToByteArray(), iv.HexToByteArray(), data.HexToByteArray()).ByteArrayToHex();
        }
        public static byte[] TripleDES_CBC_Decrypt(byte[] key, byte[] iv, byte[] data)
        {
            return TripleDES_Decrypt(key, iv, data, CipherMode.CBC);
        }

        static byte[] TripleDES_Encrypt(byte[] key, byte[] iv, byte[] data, CipherMode mode)
        {
            System.Security.Cryptography.TripleDES tripleDES = System.Security.Cryptography.TripleDES.Create();
            tripleDES.Mode = mode;
            tripleDES.Padding = PaddingMode.Zeros;

            byte[] allKey = new byte[24];
            Buffer.BlockCopy(key, 0, allKey, 0, 16);
            Buffer.BlockCopy(key, 0, allKey, 16, 8);

            ICryptoTransform trans = tripleDES.CreateEncryptor(allKey, iv);

            return trans.TransformFinalBlock(data, 0, data.Length);
        }

        static byte[] TripleDES_Decrypt(byte[] key, byte[] iv, byte[] data, CipherMode mode)
        {
            System.Security.Cryptography.TripleDES tripleDES = System.Security.Cryptography.TripleDES.Create();
            tripleDES.Mode = mode;
            tripleDES.Padding = PaddingMode.Zeros;

            byte[] allKey = new byte[24];
            Buffer.BlockCopy(key, 0, allKey, 0, 16);
            Buffer.BlockCopy(key, 0, allKey, 16, 8);

            ICryptoTransform trans = tripleDES.CreateDecryptor(allKey, iv);

            return trans.TransformFinalBlock(data, 0, data.Length);
        }
    }
}
