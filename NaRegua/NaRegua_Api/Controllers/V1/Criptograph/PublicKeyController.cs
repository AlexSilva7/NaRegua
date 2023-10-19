using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Providers.Implementations;
using System.Text;

namespace NaRegua_Api.Controllers.V1.Criptograph
{
    [Route("v1/[controller]")]
    public class PublicKeyController : Controller
    {
        private readonly ILogger<PublicKeyController> _logger;
        private readonly RSACriptograph _rsaCriptograph;

        public PublicKeyController(ILogger<PublicKeyController> logger)
        {
            _logger = logger;
            _rsaCriptograph = RSACriptograph.GetInstance();
        }

        [HttpGet]
        public IActionResult GetPublicKey()
        {
            var publicKey = _rsaCriptograph.GetPublicKey();

            return Ok(new
            {
                PublicKey = publicKey
            });
        }

        [HttpPost]
        public IActionResult TestandoApi([FromBody] Testando teste)
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(teste.Teste);
            var content = _rsaCriptograph.Decrypt(dataToEncrypt);

            return Ok(new
            {
                Content = content
            });
        }
    }
}