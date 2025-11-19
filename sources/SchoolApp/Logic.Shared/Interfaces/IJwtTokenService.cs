using Data.Entities.User;
using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IJwtTokenService: IDisposable
    {
        (string Jwt, string RefreshToken) GenerateTokens(AppUserEntity user);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request);
    }
}
