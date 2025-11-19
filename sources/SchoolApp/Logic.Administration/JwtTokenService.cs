using Data.Entities.User;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Enums;
using Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Logic.Administration
{
    public class JwtTokenService : IJwtTokenService
    {
        private bool disposedValue;
        private readonly IOptions<JwtTokenModel> _jwtOptions;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;
        
        public JwtTokenService(IOptions<JwtTokenModel> jwtOptions, IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql)
        {
            _jwtOptions = jwtOptions;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
        }

        public (string Jwt, string RefreshToken) GenerateTokens(AppUserEntity user)
        {
            return (GenerateJwt(user), GenerateRefreshToken());
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity!.Name;
            
            var user = await _applicationUnitOfWorkMySql.UserRepository.Find(x => x.Username == username);

            if (user == null || user.RefreshToken != request.RefreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var newAccessToken = GenerateJwt(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            
            _applicationUnitOfWorkMySql.UserRepository.Update(user);

            await _applicationUnitOfWorkMySql.SaveChangesAsync();

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
                new Claim(ClaimTypes.Name, appUserEntity.Username),
                new Claim(ClaimTypes.Role, appUserEntity?.UserRole.ToString() ?? UserRoleEnum.None.ToString()),
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

                ValidateLifetime = false // <= IMPORTANT: allow expired access token
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken)
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   
                }

               
                disposedValue = true;
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
