using System;
using System.Text;
using System.Security.Cryptography;

namespace Catzilla.CommonModule.Util {
    public static class SecurityUtils {
        private static readonly RijndaelManaged rijndael =
            new RijndaelManaged();

        static SecurityUtils() {
            rijndael.Mode = CipherMode.ECB;
            rijndael.Padding = PaddingMode.PKCS7;
        }

        public static string Encrypt(string input, byte[] key) {
            rijndael.Key = key;
            ICryptoTransform cryptoTransform = rijndael.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] output = cryptoTransform.TransformFinalBlock(
                inputBytes, 0, inputBytes.Length);
            return Convert.ToBase64String(output, 0, output.Length);
        }

        public static string Decrypt(string input, byte[] key) {
            rijndael.Key = key;
            ICryptoTransform cryptoTransform = rijndael.CreateDecryptor();
            byte[] inputBytes = Convert.FromBase64String(input);
            byte[] output = cryptoTransform.TransformFinalBlock(
                inputBytes, 0, inputBytes.Length);
            return Encoding.UTF8.GetString(output);
        }
    }
}
