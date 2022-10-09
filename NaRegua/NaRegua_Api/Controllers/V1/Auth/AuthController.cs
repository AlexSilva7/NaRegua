using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Auth;
using static NaRegua_Api.Models.Auth.Requests;

namespace NaRegua_Api.Controllers.V1.Auth
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
                if (Validations.ChecksIfIsNullProperty(request)) return BadRequest(new { Generic.MESSAGE });

               _logger.LogDebug($"AuthController::SignAsync - Request: {request}");
                var result = await _provider.SignAsync(request.ToDomain());

                if (!result.Success)
                {
                    _logger.LogError("AuthController::SignAsync - Não foi possivel realizar o login.");
                    return BadRequest(result);
                }
                
                var response = result.ToResponse();
                _logger.LogInformation($"AuthController::SignAsync - Response: {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{request} - {ex}");
                return Problem(ex.Message);
            }
        }
    }
}
