using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var publicKey = await GetPublicKeyApi();

        var responseWithInvalidBody = await PostInvalidDataBody(publicKey);
        var contentWithInvalidBody = responseWithInvalidBody.Content.ReadAsStringAsync();

        var responseWithEncrypt = await PostWithEncryptBody(publicKey);
        var contentWithEncrypt = await responseWithEncrypt.Content.ReadAsStringAsync();

        var responseWithoutEncrypt = await PostWithOutEncryptBody();
        var contentWithoutEncrypt = await responseWithoutEncrypt.Content.ReadAsStringAsync();
    }

    public static byte[] Encrypt(string content, string key)
    {
        using (var rsa = RSA.Create())
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(content);
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(key), out _);

            var encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
            return encryptedData;
        }
    }

    public async static Task<string?> GetPublicKeyApi()
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync("https://localhost:3000/v1/publickey");
            var result = await response.Content.ReadAsStringAsync();

            var jsonObject = JsonConvert.DeserializeObject<JObject>(result);
            var publicKey = jsonObject?["publicKey"]?.ToString();

            return publicKey;
        }
    }

    public static async Task<HttpResponseMessage> PostWithEncryptBody(string publicKey)
    {
        var obj = new
        {
            name = "leskfp",
            document = "14120252736",
            phone = "(21) 979522952",
            email = "teste@gmail.com",
            username = "leskfp",
            password = "123456",
        };

        using (var httpClient = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:3000/v1/user");
            request.Headers.Add("X-Encryption", "RSA");

            var json = JsonConvert.SerializeObject(obj);

            var encrypyBody = Encrypt(json, publicKey);
            var byteArrayContent = new ByteArrayContent(encrypyBody);
            request.Content = byteArrayContent;

            var response = await httpClient.SendAsync(request);
            return response;
        }
    }

    public static async Task<HttpResponseMessage> PostWithOutEncryptBody()
    {
        var obj = new
        {
            name = "leskfp",
            document = "14120252736",
            phone = "(21) 979522952",
            email = "teste@gmail.com",
            username = "leskfp",
            password = "123456",
        };

        using (var httpClient = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:3000/v1/user");

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await httpClient.SendAsync(request);
            return response;
        }
    }

    public static async Task<HttpResponseMessage> PostInvalidDataBody(string publicKey)
    {
        var obj = "InvalidBody";

        using (var httpClient = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:3000/v1/user");
            request.Headers.Add("X-Encryption", "RSA");
            var json = JsonConvert.SerializeObject(obj);

            var encrypyBody = Encrypt(json, publicKey);
            var byteArrayContent = new ByteArrayContent(encrypyBody);
            request.Content = byteArrayContent;

            var response = await httpClient.SendAsync(request);
            return response;
        }
    }
}