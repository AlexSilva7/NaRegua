using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaRegua_API.Common.Contracts;
using NaRegua_API.Models.Auth;
using System;
using System.Threading.Tasks;
using static NaRegua_API.Models.Auth.Requests;

namespace NaRegua_API.Controllers.V1.Auth
{
    [Route("v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthProvider _provider;

        public AuthController(ILogger<AuthController> logger, IAuthProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        [HttpPost("sign")] // POST /v1/auth/sign
        public async Task<IActionResult> SignAsync([FromBody] AuthRequest request)
        {
            try
            {
                _logger.LogDebug($"AuthController::SignAsync");
                var result = await _provider.SignAsync(request.ToDomain());

                if (!result.Success)
                {
                    _logger.LogError("AuthController::SignAsync - Não foi possivel realizar o login.");
                    return BadRequest(result);
                }
                
                var response = result.ToResponse();
                _logger.LogInformation("AuthController::SignAsync");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }
    }
}
