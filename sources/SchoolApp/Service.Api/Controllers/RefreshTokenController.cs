using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models;

namespace Service.Api.Controllers
{
    public class RefreshTokenController: ApiControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IOptions<JwtTokenModel> _jwtTokenModel;

        public RefreshTokenController(IJwtTokenService jwtTokenService, IOptions<JwtTokenModel> jwtTokenModel)
        {
            _jwtTokenService = jwtTokenService;
            _jwtTokenModel = jwtTokenModel;
        }

        [JwtAuthAttribute(UserRole = UserRoleEnum.Admin)]
        
        [HttpPost(Name = "RefreshToken")]
        public async Task<RefreshTokenResponse> RefreshToken([FromBody]RefreshTokenRequest request)
        {
            return await _jwtTokenService.RefreshToken(request);
        }
    }
}
