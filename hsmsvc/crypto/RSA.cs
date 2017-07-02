using HSMSvc.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HSMSvc.Crypto
{
    /// <summary>
    /// RSA算法加解密类
    /// </summary>
    static class RSA
    {
        public static string RSA_Encrypt(string data)
        {
            return RSA_Encrypt(data.HexToByteArray()).ByteArrayToHex();
        }
        public static byte[] RSA_Encrypt(byte[] data)
        {
            System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();

            return rsa.Encrypt(data, System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
        }

        public static string RSA_Decrypt(string data)
        {
            return RSA_Decrypt(data.HexToByteArray()).ByteArrayToHex();
        }
        public static byte[] RSA_Decrypt(byte[] data)
        {
            System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();

            return rsa.Decrypt(data, System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
        }

        public static string RSA_SignData(string data)
        {
            return RSA_SignData(data.HexToByteArray()).ByteArrayToHex();
        }
        public static byte[] RSA_SignData(byte[] data)
        {
            System.Security.Cryptography.RSA rsa = System.Security.Cryptography.RSA.Create();

            return rsa.SignData(data, System.Security.Cryptography.HashAlgorithmName.MD5, System.Security.Cryptography.RSASignaturePadding.Pkcs1);
        }
    }
}
