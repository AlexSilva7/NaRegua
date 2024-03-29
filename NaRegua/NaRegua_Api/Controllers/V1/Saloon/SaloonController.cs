﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Saloon;

namespace NaRegua_Api.Controllers.V1.Saloon
{
    [Route("v1/[controller]")]
    public class SaloonController : Controller
    {
        private readonly ILogger<SaloonController> _logger;
        private readonly ISaloonProvider _provider;

        public SaloonController(ILogger<SaloonController> logger, ISaloonProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }
        
        [Authorize]
        [HttpGet] // GET /v1/saloon/
        public async Task<IActionResult> GetSaloonsAsync()
        {
            try
            {
                if (!Validations.IsCustomer(User)) return NotFound();

                _logger.LogDebug($"SaloonController::GetSaloonsAsync");
                var result = await _provider.GetSaloonsAsync();

                if (!result.Success)
                {
                    _logger.LogError("SaloonController::GetSaloonsAsync - Não foi possivel encontrar a lista de salões.");
                    return BadRequest(result);
                }

                var response = result.ToResponse();
                _logger.LogInformation($"SaloonController::GetSaloonsAsync - {response}");

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
