using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaRegua_API.Common.Contracts;
using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Users;
using System;
using System.Threading.Tasks;
using static NaRegua_API.Models.Users.Requests;

namespace NaRegua_API.Controllers.V1.Users
{
    [Route("v1/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserProvider _provider;

        public UserController(ILogger<UserController> logger, IUserProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        [HttpPost] // POST /v1/user
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest user)
        {
            try
            {
                _logger.LogDebug($"UserController::AddUserAsync");
                var result = await _provider.CreateUserAsync(user.ToDomain());

                if (!result.Success)
                {
                    _logger.LogError("UserController::AddUserAsync - Não foi possivel cadastrar o usuário.");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation("UserController::AddUserAsync");

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
