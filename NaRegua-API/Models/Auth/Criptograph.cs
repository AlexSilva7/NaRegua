using System;
using System.Security.Cryptography;
using System.Text;

namespace NaRegua_API.Models.Auth
{
    public static class Criptograph
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public static string HashKey(string key)
        {
            byte[] salt = new byte[16];
            rngCsp.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(key, salt, 1000);

            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var hashPassword = Convert.ToBase64String(hashBytes);

            return hashPassword;
        }

        public static string HashPass(string password)
        {
            var encoding = new UnicodeEncoding();
            byte[] hashBytes;

            using (HashAlgorithm hash = SHA1.Create())
            {
                hashBytes = hash.ComputeHash(encoding.GetBytes(password));
            }

            var hashPassword = Convert.ToBase64String(hashBytes);
            return hashPassword;
        }
    }
}
