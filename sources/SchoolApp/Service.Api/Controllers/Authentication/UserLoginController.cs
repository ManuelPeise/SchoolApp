using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Service.Api.Controllers.Authentication
{
    public class UserLoginController: ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public UserLoginController(IAuthenticationService authenticationService)
        {
           _authenticationService = authenticationService;
        }

        //api/userloginC/login
        [HttpPost(Name = "Login")]
        public async Task<LoginResult> Login([FromBody]LoginRequestModel model)
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
