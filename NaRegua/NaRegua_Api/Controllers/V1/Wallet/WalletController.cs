using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Users;
using static Google.Rpc.Context.AttributeContext.Types;
using static NaRegua_Api.Models.Users.Requests;

namespace NaRegua_Api.Controllers.V1.Wallet
{
    [Route("v1/[controller]")]
    public class WalletController : Controller
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IUserProvider _userProvider;
        public WalletController(ILogger<WalletController> logger, IUserProvider userProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
        }

        [Authorize]
        [HttpPost("add-funds")]
        public async Task<IActionResult> PostAddFundsAccountAsync([FromBody] DepositsFundsRequests request)
        {
            if (Validations.ChecksIfIsNullProperty(request)) return BadRequest(new { GenericMessage.INCOMPLETE_FIELDS });

            var result = await _userProvider.DepositFundsAsync(User, request.ToDomain());
            return Ok(result);
        }

        [Authorize]
        [HttpGet("balance-account")]
        public async Task<IActionResult> GetBalanceAccountAsync()
        {
            var result = await _userProvider.GetAccountBalanceAsync(User);
            return Ok(result);
        }
    }
}
