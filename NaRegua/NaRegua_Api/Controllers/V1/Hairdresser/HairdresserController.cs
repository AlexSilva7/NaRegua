using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Hairdresser;
using static NaRegua_Api.Models.Hairdresser.Requests;

namespace NaRegua_Api.Controllers.V1.Hairdresser
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
                if (request.ChecksIfIsNullProperty()) return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });
                if (!request.Document.VerifyIfIsValidCpf()) return BadRequest(new { GenericMessage.INVALID_DOCUMENT });
                if (!request.Email.VerifyIfIsValidEmail()) return BadRequest(new { GenericMessage.INVALID_EMAIL });

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
                if (Validations.IsCustomer(User)) return NotFound();

                if (Validations.ChecksIfIsNullProperty(request)) return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });

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
                if (!Validations.IsCustomer(User)) return NotFound();

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
        [HttpGet("professional-appointments")] // GET /v1/hairdresser/professional-appointments
        public async Task<IActionResult> GetAppointmentsFromTheProfessional()
        {
            try
            {
                if (Validations.IsCustomer(User)) return NotFound();

                _logger.LogDebug($"HairdresserController::GetAppointmentsFromTheProfessional - Request");
                var result = await _provider.GetAppointmentsFromTheProfessional(User);

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::GetAppointmentsFromTheProfessional - " +
                        "Não foi possivel consultar a disponibilidade do Profissional.");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::GetAppointmentsFromTheProfessional - Response: {response.Resources}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("evaluation-average/{document}")] // GET /v1/hairdresser/evaluation-average
        public async Task<IActionResult> GetEvaluationAverage(string document)
        {
            try
            {
                _logger.LogDebug($"HairdresserController::GetEvaluationAverage - Request: {document}");
                var result = await _provider.GetEvaluationAverageFromTheProfessional(document);

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::GetEvaluationAverage - " +
                        "Não foi possivel consultar a disponibilidade do Profissional. - Result: " + result);
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::GetEvaluationAverage - Response: {response}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Problem(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("evaluation-average")] // POST /v1/hairdresser/evaluation-average
        public async Task<IActionResult> SendEvaluationAverage([FromBody] ProfessionalEvaluationRequest request)
        {
            try
            {
                if (!Validations.IsCustomer(User)) return NotFound();

                if(Validations.ChecksIfIsNullProperty(request)) return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });

                _logger.LogDebug($"HairdresserController::SendEvaluationAverage - Request: {request}");
                var result = await _provider.SendEvaluationAverageFromTheProfessional(request.ToDomain());

                if (!result.Success)
                {
                    _logger.LogError("HairdresserController::SendEvaluationAverage - " +
                        "Não foi possivel consultar a disponibilidade do Profissional. - Result: " + result.Message);
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"HairdresserController::SendEvaluationAverage - Response: {response.Message}");

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
