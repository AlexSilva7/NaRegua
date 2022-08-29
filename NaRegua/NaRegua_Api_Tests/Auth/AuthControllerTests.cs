using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using NaRegua_Api.Controllers.V1.Auth;
using NaRegua_Api.Models.Users;
using NaRegua_Api.Providers.Fakes;
using NaRegua_Api.Providers.Implementations.Token;
using System;
using System.Threading.Tasks;
using Xunit;
using static NaRegua_Api.Models.Auth.Requests;
using static NaRegua_Api.Models.Auth.Responses;

namespace NaRegua_Api_Tests.Auth
{
    public class SaloonControllerTests
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ISaloonProvider _salonProvider;
        private readonly ITokenProvider _tokenProvider;
        private readonly IHairdresserProvider _hairdresserProvider;
        private readonly IUserProvider _userProvider;
        private readonly IAuthProvider _authProvider;

        public SaloonControllerTests()
        {
            AppSettings.JwtKey = "minhachavesecretadeteste";
            AppSettings.ExpiryDurationMinutes = 10;
            _logger = new LoggerFake();
            _salonProvider = new SaloonProviderFake();
            _tokenProvider = new TokenProvider();
            _hairdresserProvider = new HairdresserProviderFake(_salonProvider);
            _userProvider = new UserProviderFake(_hairdresserProvider, _salonProvider);
            _authProvider = new AuthProviderFake(_tokenProvider, _userProvider, _hairdresserProvider);
        }

        [Fact]
        public async Task SignAsyncSuccessfully()
        {
            var create = _userProvider.CreateUserAsync(new User
            {
                Name = "teste",
                Document = "teste",
                Phone = "2145478",
                Email = "dsadsa@teste.com.br",
                Username = "teste",
                Password = "teste"
            });

            var authController = new AuthController(_logger, _authProvider);

            var request = new AuthRequest
            {
                Username = "teste",
                Password = "teste"
            };

            var response = await authController.SignAsync(request);

            var result = Assert.IsType<OkObjectResult>(response);
            var returnValue = Assert.IsType<AuthResponse>(result.Value);
            var username = returnValue.Resources.Username;

            Assert.Equal("teste", username);
        }

        [Fact]
        public async Task SignAsyncNotSuccessfully_UserNotFound()
        {
            var authController = new AuthController(_logger, _authProvider);

            var request = new AuthRequest
            {
                Username = "teste",
                Password = "teste"
            };

            var response = await authController.SignAsync(request);

            var result = Assert.IsType<BadRequestObjectResult>(response);
            var returnValue = Assert.IsType<AuthResult>(result.Value);
            var message = returnValue.Message;

            Assert.Equal("User not found", message);
        }

        [Fact]
        public async Task SignAsyncNotSuccessfully_LoginWithoutUsername()
        {
            var authController = new AuthController(_logger, _authProvider);

            var request = new AuthRequest
            {
                Password = "teste"
            };

            var response = await authController.SignAsync(request);

            var result = Assert.IsType<BadRequestObjectResult>(response);
            var returnValue = Assert.IsType<AuthResult>(result.Value);
            var message = returnValue.Message;

            Assert.Equal("Unable to login, incomplete fields", message);
        }

        [Fact]
        public async Task SignAsyncNotSuccessfully_LoginWithoutPassword()
        {
            var authController = new AuthController(_logger, _authProvider);

            var request = new AuthRequest
            {
                Username = "teste"
            };

            var response = await authController.SignAsync(request);

            var result = Assert.IsType<BadRequestObjectResult>(response);
            var returnValue = Assert.IsType<AuthResult>(result.Value);
            var message = returnValue.Message;

            Assert.Equal("Unable to login, incomplete fields", message);
        }
    }

    public class LoggerFake : ILogger<AuthController>
    {
        public IDisposable BeginScope<TState>(TState state) { throw new NotImplementedException(); }
        public bool IsEnabled(LogLevel logLevel) { throw new NotImplementedException(); }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { return; }
    }
}




