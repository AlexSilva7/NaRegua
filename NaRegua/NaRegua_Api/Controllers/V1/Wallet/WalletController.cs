using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Enums;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Users;
using static NaRegua_Api.Models.Users.Requests;

namespace NaRegua_Api.Controllers.V1.Wallet
{
    [Route("v1/[controller]")]
    public class WalletController : Controller
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IUserProvider _userProvider;
        private readonly IHairdresserProvider _hairdresserProvider;
        public WalletController(ILogger<WalletController> logger, IUserProvider userProvider, 
            IHairdresserProvider hairdresserProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
            _hairdresserProvider = hairdresserProvider;
        }

        [Authorize]
        [HttpPost("add-funds")]
        public async Task<IActionResult> PostAddFundsAccountAsync([FromBody] DepositsFundsRequests request)
        {
            if (request?.PaymentType is null || request.Value == 0) 
                return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });

            if (request.PaymentType == PaymentType.Credit && string.IsNullOrEmpty(request.CardNumber))
                return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });

            var result = await _userProvider.DepositFundsAsync(User, request.ToDomain());
            return Ok(result);
        }

        [Authorize]
        [HttpGet("balance-account")]
        public async Task<IActionResult> GetBalanceAccountAsync()
        {
            var IsCustomer = Validations.FindFirstClaimOfType(User, "IsCustomer");

            if (bool.Parse(IsCustomer))
            {
                return Ok(await _userProvider.GetAccountBalanceAsync(User));
            }
            else
            {
                return Ok(await _hairdresserProvider.GetAccountBalanceAsync(User));
            }
        }
    }
}
