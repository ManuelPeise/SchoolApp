using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Service.Api.Controllers.Authentication
{
    [JwtAuth(UserRoleString = "User, Admin, SystemAdmin")]
    public class AuthenticationController: ApiControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IOptions<JwtTokenModel> _jwtTokenModel;

        public AuthenticationController(
            IJwtTokenService jwtTokenService, 
            IOptions<JwtTokenModel> jwtTokenModel)
        {
            _jwtTokenService = jwtTokenService;
            _jwtTokenModel = jwtTokenModel;
        }

        [HttpPost(Name = "RefreshToken")]
        public async Task<RefreshTokenResponse> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            return await _jwtTokenService.RefreshToken(request);
        }
    }
}
