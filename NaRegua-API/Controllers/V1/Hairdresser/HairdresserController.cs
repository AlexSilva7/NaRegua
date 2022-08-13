using Microsoft.AspNetCore.Authorization;
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
                        "Não foi possivel cadastrar o Profissional. ::" + result);
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

        [Authorize]
        [HttpPost("send-work-availability")] // POST /v1/hairdresser/send-work-availability
        public async Task<IActionResult> SendWorkAvailabilityAsync([FromBody] WorkAvailabilityRequest request)
        {
            try
            {
                _logger.LogDebug($"HairdresserController::SendWorkAvailability - {request}");
                var result = await _provider.SendWorkAvailabilityAsync(request.ToDomain(), User);

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::SendWorkAvailabilityAsync - " +
                        "Não foi possivel cadastrar a disponibilidade do Profissional. ::" + result);
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

        [Authorize]
        [HttpGet("hairdressers-list/{salonCode}")] // GET /v1/hairdresser/hairdressers-list/057952B
        public async Task<IActionResult> GetHairdressersListOfSalonAsync(string salonCode)
        {
            try
            {
                _logger.LogDebug($"HairdresserController::GetHairdressersListOfSalon - Request: {salonCode}");
                var result = await _provider.GetHairdressersListOfSalon(salonCode);

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::GetHairdressersListOfSalon - " +
                        "Não foi possivel consultar a lista de Profissionais. :: Result: " + result);
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::GetHairdressersListOfSalon - Response: {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("professional-availability/{document}")] // GET /v1/hairdresser/professional-availability/65240069707
        public async Task<IActionResult> GetProfessionalAvailability(string document)
        {
            try
            {
                _logger.LogDebug($"HairdresserController::GetProfessionalAvailability - Request: {document}");
                var result = await _provider.GetProfessionalAvailability(document);

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::GetProfessionalAvailability - " +
                        "Não foi possivel consultar a disponibilidade do Profissional. - Result: " + result);
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::GetProfessionalAvailability - Response: {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("professional-appointments/{document}")] // GET /v1/hairdresser/professional-appointments/65240069707
        public async Task<IActionResult> GetAppointmentsFromTheProfessional(string document)
        {
            try
            {
                _logger.LogDebug($"HairdresserController::GetAppointmentsFromTheProfessional - Request: {document}");
                var result = await _provider.GetAppointmentsFromTheProfessional(document);

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::GetAppointmentsFromTheProfessional - " +
                        "Não foi possivel consultar a disponibilidade do Profissional. - Result: " + result);
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::GetAppointmentsFromTheProfessional - Response: {response}");

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
