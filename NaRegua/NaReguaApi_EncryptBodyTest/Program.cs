using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

internal class Program
{
    private static async Task Main(string[] args)
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

        var httpClient = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:3000/v1/user");
        request.Headers.Add("X-Encryption", "X-Encryption");

        var json = JsonConvert.SerializeObject(obj);
        var encrypyBody = Encrypt(json);
        var byteArrayContent = new ByteArrayContent(encrypyBody);
        request.Content = byteArrayContent;

        var responseCreateUser = await httpClient.SendAsync(request);
        var responseCreateUserX = responseCreateUser.Content.ReadAsStringAsync();
    }

    public static byte[] Encrypt(string content)
    {
        using (var rsa = RSA.Create())
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(content);
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String("MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA62JCJZ9oFsAq1lzJ3p5JfcsxCblwKKSiOTWodc8liWnQPAiHxfvnzQIl3E7hZpfN/jnJYCfFWz+6sjVlfzgwfcPU0a2Kt6kVARQUqYbbTnacGOjpynNH0kRPLfMzeOY6+sijmeilVq04fwiLd4WdjX0fmQkQw4y93xAJGREHqYfumYWiYP6YejZP4humTOh0/TKqzyolkNQ8QsU0ZKZ72/hZHRuXrhghZd5vVH2z/16zm8Oa/ijiaqfLQARtL6n8JXl4t2zc79fOo5tQ7+jYU82LkqTl+PzwMH3UZWFJMK6M2RMnBsfIXNKJNLkRIooXj+rfSqgvJ96Vj/4tykNk6kMWoX8XlHIttSKKiamelDZQId5MuOpsze0uLbOwoGq6oNLqK37ZBsTO2COhN4Z3AyjWPNsgAdo4Ca4MN/N3hXl2TRYUTzroPgCxka58NeofLlcia5KMZNa+nWaoih0oKc+4QGsAJyo/Bw7yE0K5PDiTyYjL3V7QYmtmE1pVTeeKPPxi55RMEnieEBDrkoz7KkrxhqOSeOTv597QqLd/3i1XfOj3Ih2Uc/seSC4o9NRaXIeM+lTAHtYT9WKGA54qubPg5K9GTiFWYdDf71Mlj8ih6bdqTcDkKPq3PjwAgewp7j+qwv27Qe/5TxNc+BootLLL0bwcelpYQdK2sXFXp/ECAwEAAQ=="), out _);

            var encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
            return encryptedData;
        }
    }
}


//var responseString = await publicKeyResponse.Content.ReadAsStringAsync();