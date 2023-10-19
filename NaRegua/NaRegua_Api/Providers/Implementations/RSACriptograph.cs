using System.Reflection;
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
        private static RSACriptograph instance;
        private static readonly object lockObject = new object();

        private readonly string? ExecutablePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        public RSACriptograph()
        {
            using (var rsa = RSA.Create(4096))
            {
                var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                File.WriteAllText($"{ExecutablePath}/Pem/private_key.pem", privateKey);

                var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
                File.WriteAllText($"{ExecutablePath}/Pem/public_key.pem", publicKey);
            }
        }

        public static RSACriptograph GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new RSACriptograph();
                    }
                }
            }
            return instance;
        }

        public string GetPublicKey()
        {
            var publicKey = File.ReadAllText($"{ExecutablePath}/Pem/public_key.pem");
            return publicKey;
        }

        public string GetPrivateKey()
        {
            var privateKey = File.ReadAllText($"{ExecutablePath}/Pem/private_key.pem");
            return privateKey;
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
                var privateKey = File.ReadAllText($"{ExecutablePath}/Pem/public_key.pem");
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

                var dataDecript = rsa.Decrypt(content, RSAEncryptionPadding.Pkcs1);
                string decryptedData = Encoding.UTF8.GetString(dataDecript);

                return decryptedData;
            }
        }
    }
}
