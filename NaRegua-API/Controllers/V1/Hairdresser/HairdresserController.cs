using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaRegua_API.Common.Contracts;
using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Hairdresser;
using System;
using System.Threading.Tasks;
using static NaRegua_API.Models.Hairdresser.Requests;

namespace NaRegua_API.Controllers.V1.Hairdresser
{
    [Route("v1/[controller]")]
    public class HairdresserController : Controller
    {
        private readonly ILogger<HairdresserController> _logger;
        private readonly IHairdresserProvider _provider;

        public HairdresserController(ILogger<HairdresserController> logger, IHairdresserProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        [HttpPost] // POST /v1/hairdresser
        public async Task<IActionResult> CreateHairdresserAsync([FromBody] HairdresserRequest request)
        {
            try
            {
                _logger.LogDebug($"HairdresserController::CreateProfessionalAsync - {request}");
                var result = await _provider.CreateHairdresserAsync(request.ToDomain());

                if (!result.Success)
                {
                    _logger.LogError(
                        "HairdresserController::CreateProfessionalAsync - " +
                        "Não foi possivel cadastrar o Profissional.");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::CreateProfessionalAsync - {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [HttpPost("send-work-availability")] // POST /v1/hairdresser
        public async Task<IActionResult> SendWorkAvailabilityAsync([FromBody] WorkAvailabilityRequest request)
        {
            try
            {
                _logger.LogDebug($"HairdresserController::SendWorkAvailability - {request}");
                var result = await _provider.SendWorkAvailabilityAsync(request.ToDomain());

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::SendWorkAvailabilityAsync - " +
                        "Não foi possivel cadastrar a disponibilidade do Profissional.");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::SendWorkAvailability - {response}");

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
