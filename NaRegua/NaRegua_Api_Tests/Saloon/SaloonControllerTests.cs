using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using NaRegua_Api.Controllers.V1.Saloon;
using NaRegua_Api.Providers.Fakes;
using NaRegua_Api.Providers.Implementations.Token;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NaRegua_Api_Tests.Saloon
{
    public class SaloonControllerTests
    {
        private readonly ILogger<SaloonController> _logger;
        private readonly ISaloonProvider _salonProvider;
        private readonly ITokenProvider _tokenProvider;

        public SaloonControllerTests()
        {
            AppSettings.JwtKey = "minhachavesecretadeteste";
            AppSettings.ExpiryDurationMinutes = 10;
            _logger = new LoggerFake();
            _salonProvider = new SaloonProviderFake();
            _tokenProvider = new TokenProvider();
        }

        [Fact]
        public async Task GetSaloonsAsyncSuccessfully()
        {
            var saloonController = new SaloonController(_logger, _salonProvider);

            var response = await saloonController.GetSaloonsAsync();

            var result = Assert.IsType<UnauthorizedObjectResult>(response);
            var returnValue = Assert.IsType<AuthResult>(result.Value);
            var message = returnValue.Message;

            Assert.Equal("Unable to login, incomplete fields", message);
        }
    }

    public class LoggerFake : ILogger<SaloonController>
    {
        public IDisposable BeginScope<TState>(TState state) { throw new NotImplementedException(); }
        public bool IsEnabled(LogLevel logLevel) { throw new NotImplementedException(); }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { return; }
    }
}




