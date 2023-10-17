using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Users;
using static NaRegua_Api.Models.Users.Requests;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Common.Enums;
using NaRegua_Api.QueueService;

namespace NaRegua_Api.Controllers.V1.Users
{
    [Route("v1/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserProvider _provider;
        private readonly IOrderProvider _orderProvider;

        public UserController(ILogger<UserController> logger, IUserProvider provider, 
            IOrderProvider orderProvider)
        {
            _logger = logger;
            _provider = provider;
            _orderProvider = orderProvider;
        }

        [HttpPost] // POST /v1/user
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest request)
        {
            try
            {
                if (request.ChecksIfIsNullProperty()) return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });
                if (!request.Document.VerifyIfIsValidCpf()) return BadRequest(new { GenericMessage.INVALID_DOCUMENT });
                if (!request.Email.VerifyIfIsValidEmail()) return BadRequest(new { GenericMessage.INVALID_EMAIL });

                _logger.LogDebug($"UserController::AddUserAsync");

                var result = await _provider.CreateUserAsync(request.ToDomain());

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
            try
            {
                if (Validations.ChecksIfIsNullProperty(request)) return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });

                if (!Validations.IsCustomer(User)) return NotFound();

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
            try
            {
                if (!Validations.IsCustomer(User)) return NotFound();

                _logger.LogDebug($"UserController::GetAppointmentAsync");
                var result = await _provider.GetAppointmentAsync(User);

                if (!result.Success)
                {
                    _logger.LogError($"UserController::GetAppointmentAsync - {result.Message}");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"UserController::GetAppointmentAsync - {response.Resources}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("favorite-salon")] //POST /v1/user/favorites-salon
        public async Task<IActionResult> GetFavoriteSaloonsAsync()
        {
            try
            {
                if (!Validations.IsCustomer(User)) return NotFound();

                _logger.LogDebug($"UserController::GetFavoriteSaloonsAsync");
                var result = await _provider.GetUserFavoriteSaloonsAsync(User);

                if (!result.Success)
                {
                    _logger.LogError($"UserController::GetFavoriteSaloonsAsync - {result.Message}");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"UserController::GetFavoriteSaloonsAsync - {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("favorite-salon")] //POST /v1/user/favorite-salon
        public async Task<IActionResult> PostSalonAsFavoriteAsync([FromBody] AddFavoriteRequest request)
        {
            try
            {
                if (!Validations.IsCustomer(User)) return NotFound();

                if (Validations.ChecksIfIsNullProperty(request)) return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });

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

        [Authorize]
        [HttpDelete("favorite-salon/{saloonCode}")] //DELETE /v1/user/favorite-salon/xxxx
        public async Task<IActionResult> RemoveSalonFromFavoritesAsync(string saloonCode)
        {
            try
            {
                if (!Validations.IsCustomer(User)) return NotFound();

                _logger.LogDebug($"UserController::RemoveSalonFromFavoritesAsync");
                var result = await _provider.RemoveSalonFromFavoritesAsync(User, saloonCode);

                if (!result.Success)
                {
                    _logger.LogError($"UserController::RemoveSalonFromFavoritesAsync - {result.Message}");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"UserController::RemoveSalonFromFavoritesAsync - {response.Success}");

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
