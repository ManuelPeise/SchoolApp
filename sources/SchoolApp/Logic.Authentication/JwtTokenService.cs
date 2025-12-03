using Data.Entities.User;
using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Enums;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Logic.Authentication
{
    public class JwtTokenService : IJwtTokenService
    {
        private bool _disposedValue;
        private readonly IOptions<JwtTokenModel> _jwtOptions;
        private readonly IRemoteDatabaseAccessor _databaseAccessor;

        public JwtTokenService(IOptions<JwtTokenModel> jwtOptions, IRemoteDatabaseAccessor databaseAccessor)
        {
            _jwtOptions = jwtOptions;
            _databaseAccessor = databaseAccessor;
        }

        public (string Jwt, string RefreshToken) GenerateTokens(AppUserEntity user)
        {
            return (GenerateJwt(user), GenerateRefreshToken());
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity!.Name;

            var user = await _databaseAccessor.UserRepository.Find(x => x.UserName == username);

            if (user == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            await _databaseAccessor.UserCredentialsRepository.GetByIdAsync(user.CredentialsId);

            if (user.Credentials.RefreshToken != request.RefreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var newAccessToken = GenerateJwt(user);
            var newRefreshToken = GenerateRefreshToken();

            user.Credentials.RefreshToken = newRefreshToken;

            _databaseAccessor.UserCredentialsRepository.Update(user.Credentials);

            await _databaseAccessor.SaveChangesAsync();

            return new RefreshTokenResponse
            {
                JwtToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        private string GenerateJwt(AppUserEntity appUserEntity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("userId", appUserEntity.Id.ToString()),
                new Claim("name", appUserEntity.UserName),
                new Claim("userRole", appUserEntity?.UserRole.ToString() ?? UserRoleEnum.None.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(_jwtOptions.Value.ExpiresInSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtOptions.Value.Audience,

                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Value.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey)
                ),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _databaseAccessor.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
