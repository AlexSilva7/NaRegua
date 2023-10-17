using System.Security.Cryptography;
using System.Text;

namespace NaRegua_Api.Providers.Implementations
{
    public class Testando
    {
        public string Teste { get; set; }
    }

    public class RSACriptograph
    {
        private const string PrivateKeyFilePath = "private_key.pem";
        private const string PublicKeyFilePath = "public_key.pem";

        public RSACriptograph()
        {
            using(var rsa = RSA.Create(1024))
            {
                var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                File.WriteAllText(PrivateKeyFilePath, privateKey);

                var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
                File.WriteAllText(PublicKeyFilePath, publicKey);
            }
        }

        public string GetPublicKey()
        {
            var publicKey = File.ReadAllText(PublicKeyFilePath);
            return publicKey;
        }

        public byte[] Encrypt(string content)
        {
            using (var rsa = RSA.Create())
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(content);
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(GetPublicKey()), out _);

                var encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
                return encryptedData;
            }
        }

        public string Decrypt(byte[] content)
        {
            using (var rsa = RSA.Create())
            {
                var privateKey = File.ReadAllText(PrivateKeyFilePath);
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

                var dataDecript = rsa.Decrypt(content, RSAEncryptionPadding.Pkcs1);
                string decryptedData = Encoding.UTF8.GetString(dataDecript);

                return decryptedData;
            }
        }
    }
}
