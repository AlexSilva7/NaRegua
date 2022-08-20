using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Users;
using static NaRegua_Api.Models.Users.Requests;

namespace NaRegua_Api.Controllers.V1.Users
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

        
        [Authorize]
        [HttpPost("schedule-appointment")] // POST /v1/user/schedule-appointment
        public async Task<IActionResult> ScheduleAppointmentAsync([FromBody] ScheduleAppointmentRequest request)
        {
            if (!Validations.IsCustomer(User))
            {
                return NotFound();
            }

            try
            {
                _logger.LogDebug($"UserController::ScheduleAppointmentAsync");
                var result = await _provider.ScheduleAppointmentAsync(User, request.DateTime, request.DocumentProfessional);

                if (!result.Success)
                {
                    _logger.LogError($"UserController::ScheduleAppointmentAsync - {result.Message}");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation("UserController::ScheduleAppointmentAsync");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        
        [Authorize]
        [HttpGet("schedule-appointment")] // GET /v1/user/schedule-appointment
        public async Task<IActionResult> GetAppointmentAsync()
        {
            if (!Validations.IsCustomer(User))
            {
                return NotFound();
            }

            try
            {
                _logger.LogDebug($"UserController::GetAppointmentAsync");
                var result = await _provider.GetAppointmentAsync(User);

                if (!result.Success)
                {
                    _logger.LogError($"UserController::GetAppointmentAsync - {result.Message}");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"UserController::GetAppointmentAsync - {response.Resource}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("add-favorite-salon")] //POST /v1/user/add-favorite-salon
        public async Task<IActionResult> PostSalonAsFavoriteAsync([FromBody] AddFavoriteRequest request)
        {
            if (!Validations.IsCustomer(User))
            {
                return NotFound();
            }

            try
            {
                _logger.LogDebug($"UserController::PostSalonAsFavoriteAsync");
                var result = await _provider.AddUserSalonAsFavoriteAsync(User, request.saloonCode);

                if (!result.Success)
                {
                    _logger.LogError($"UserController::PostSalonAsFavoriteAsync - {result.Message}");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"UserController::PostSalonAsFavoriteAsync - {response.Success}");

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
