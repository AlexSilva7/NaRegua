using Google.Api;
using NaRegua_Api.Providers.Implementations;
using Newtonsoft.Json;
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
            // Lógica do middleware para processar a requisição antes de chegar à controller
            var request = context.Request;

            // Verificar se a requisição requer criptografia
            if (request.Headers.ContainsKey("X-Encryption"))
            {
                // Extrair o corpo criptografado da requisição
                var encryptedBody = await ExtractEncryptedBody(request);

                // Descriptografar o corpo usando o algoritmo assimétrico
                var decryptedBody = DecryptBody(encryptedBody);

                // Verificar o tipo de dado do corpo descriptografado
                //, request.Action
                if (IsValidDataType(decryptedBody))
                {
                    // Atualizar o objeto de requisição com o corpo descriptografado
                    UpdateRequestBody(request, decryptedBody);
                }
                else
                {
                    // Tipo de dado inválido, retornar erro
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Tipo de dado inválido.");
                    return;
                }
            }

            // Chamar o próximo middleware ou a rota correspondente à controller
            await _next(context);
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
            // Lógica para verificar o tipo de dado do corpo descriptografado
            // ...
            return true;
        }

        private void UpdateRequestBody(HttpRequest request, string decryptedBody)
        {
            // Codifique a string descriptografada em bytes usando a codificação adequada
            byte[] byteArray = Encoding.UTF8.GetBytes(decryptedBody);

            // Crie um novo fluxo de memória com os bytes da string
            var bodyStream = new MemoryStream(byteArray);

            // Defina o corpo da solicitação como o novo fluxo de memória
            request.Body = bodyStream;

            // Lembre-se de atualizar o cabeçalho "Content-Length" para refletir o tamanho do novo corpo
            request.ContentLength = byteArray.Length;

            request.ContentType = "application/json";

            //// Defina o tipo de conteúdo, se aplicável
            //if (!string.IsNullOrWhiteSpace(request.ContentType))
            //{
            //    request.ContentType = "application/json"; // Substitua pelo tipo de conteúdo adequado
            //}
        }
    }
}
