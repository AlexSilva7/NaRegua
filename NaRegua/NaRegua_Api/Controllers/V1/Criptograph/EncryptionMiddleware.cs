using Google.Api;
using Microsoft.AspNetCore.Http;
using NaRegua_Api.Providers.Implementations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NaRegua_Api.Controllers.V1.Criptograph
{
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RSACriptograph _rsaCriptograph;

        public EncryptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _rsaCriptograph = RSACriptograph.GetInstance();
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var path = request.Path.Value;

            if (path == "/v1/publickey")
            {
                await _next(context);
                return;
            }

            if (!request.Headers.ContainsKey("X-Encryption"))
            {
                await SetInvalidRequestResponseMessage(context.Response, "The 'X-Encryption' header is required.");
                return;
            }
            else
            {
                var encryptedBody = await ExtractEncryptedBody(request);
                var decryptedBody = DecryptBody(encryptedBody);

                if (IsValidDataType(decryptedBody))
                {
                    UpdateRequestBody(request, decryptedBody);
                    await _next(context);
                }
                else
                {
                    await SetInvalidRequestResponseMessage(context.Response, "Invalid Data Type!");
                    return;
                }
            }
        }

        private async Task<byte[]> ExtractEncryptedBody(HttpRequest request)
        {
            var bodyStream = request.Body;

            using (var memoryStream = new MemoryStream())
            {
                await bodyStream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private string DecryptBody(byte[] content)
        {
            var privateKey = _rsaCriptograph.GetPrivateKey();
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

                var dataDecript = rsa.Decrypt(content, RSAEncryptionPadding.Pkcs1);
                string decryptedData = Encoding.UTF8.GetString(dataDecript);

                return decryptedData;
            }
        }

        private bool IsValidDataType(string decryptedBody)
        {
            try
            {
                JObject.Parse(decryptedBody);
                return true;

            }
            catch (JsonException)
            {
                return false;
            }
        }

        private void UpdateRequestBody(HttpRequest request, string decryptedBody)
        {
            var byteArray = Encoding.UTF8.GetBytes(decryptedBody);
            var bodyStream = new MemoryStream(byteArray);

            request.Body = bodyStream;
            request.ContentLength = byteArray.Length;

            if (string.IsNullOrWhiteSpace(request.ContentType))
            {
                request.ContentType = "application/json"; // Substitua pelo tipo de conteúdo adequado
            }
        }

        private async Task SetInvalidRequestResponseMessage(HttpResponse httpResponse, string message)
        {
            var responseMessage = new { message = message };
            var jsonResponse = JsonConvert.SerializeObject(responseMessage);

            httpResponse.ContentType = "application/json";
            httpResponse.StatusCode = 400;

            await httpResponse.WriteAsync(jsonResponse);
        }
    }
}
