namespace NaRegua_Api.Controllers.V1.Criptograph
{
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;

        public EncryptionMiddleware(RequestDelegate next)
        {
            _next = next;
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

        // Métodos auxiliares
        // ...

        private async Task<string> ExtractEncryptedBody(HttpRequest request)
        {
            // Lógica para extrair o corpo criptografado da requisição
            // ...
            return string.Empty;
        }

        private string DecryptBody(string encryptedBody)
        {
            // Lógica para descriptografar o corpo usando o algoritmo assimétrico
            // ...
            return string.Empty;
        }

        private bool IsValidDataType(string decryptedBody)
        {
            // Lógica para verificar o tipo de dado do corpo descriptografado
            // ...
            return true;
        }

        private void UpdateRequestBody(HttpRequest request, string decryptedBody)
        {
            // Atualizar o objeto de requisição com o corpo descriptografado
            // ...
        }
    }
}
