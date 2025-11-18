using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace Service.Api.Controllers.Account
{
    public class UserLoginController: ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UserLoginController(IAuthenticationService authenticationService)
        {
           _authenticationService = authenticationService;
        }

        [HttpPost(Name = "Login")]
        public async Task<AuthenticationResult> Login(LoginRequestModel model)
        {
            return await _authenticationService.LoginAsync(model);
        }

        [HttpPost(Name = "Logout")]
        public async Task Logout()
        {
            _authenticationService.LogOut();
        }
    }
}
