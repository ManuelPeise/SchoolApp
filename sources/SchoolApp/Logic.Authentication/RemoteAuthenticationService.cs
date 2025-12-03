using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Logic.Shared.Storage;

namespace Logic.Authentication
{
    public class RemoteAuthenticationService : IAuthenticationService
    {
        private readonly IRemoteDatabaseAccessor _databaseAccessor;
        private readonly IJwtTokenService _jwtTokenService;
        private bool disposedValue;

        public RemoteAuthenticationService(IRemoteDatabaseAccessor databaseAccessor, IJwtTokenService jwtTokenService)
        {
            _databaseAccessor = databaseAccessor;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResult> LoginAsync(LoginRequestModel model)
        {
            try
            {
                var user = await _databaseAccessor.UserRepository.Find(
                    x => x.UserName.ToLower() == model.UserName.ToLower(),
                    true, e => e.Family, e => e.Credentials, e => e.Settings);

                if (user == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check username!"
                    };
                }

                var encriptedPassword = PasswordHelper.HashPassword(model.Password, user.Credentials.Salt);

                if (encriptedPassword != user.Credentials.Password)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check password!"
                    };
                }

                var tokenData = _jwtTokenService.GenerateTokens(user);

                user.Credentials.RefreshToken = tokenData.RefreshToken;
                user.Credentials.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(1);
                user.Credentials.IsInSync = true;
                
                _databaseAccessor.UserCredentialsRepository.Update(user.Credentials);

                await _databaseAccessor.SaveChangesAsync();

                return new LoginResult
                {
                    Success = true,
                    Message = "Login success!",
                    JwtToken = tokenData.Jwt,
                    AppUser = user
                };
            }
            catch (Exception)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Login failed!"
                };
            }
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                   _databaseAccessor.Dispose();
                    _jwtTokenService.Dispose();
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
