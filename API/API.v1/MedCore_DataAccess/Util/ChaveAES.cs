using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MedCore_DataAccess.Util
{
    public class ChaveAES
    {
        private static byte[] key = { 123, 217, 19, 11, 24, 26, 32, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 188, 144, 173, 53, 196, 29, 24, 26, 45, 218, 131, 36, 53, 209 };
        private static byte[] vector = { 146, 64, 191, 111, 23, 3, 154, 134, 231, 121, 221, 112, 79, 32, 14, 159 };
        private ICryptoTransform encryptor, decryptor;
        private UTF8Encoding encoder;

        public ChaveAES()
        {
            RijndaelManaged rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public string EncryptToUrl(string unencrypted)
        {
            return HttpUtility.UrlEncode(Encrypt(unencrypted));
        }

        public string DecryptFromUrl(string encrypted)
        {
            return Decrypt(HttpUtility.UrlDecode(encrypted));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }        
    }
}