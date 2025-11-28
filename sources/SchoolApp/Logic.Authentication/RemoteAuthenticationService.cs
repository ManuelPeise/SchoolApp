using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Logic.Shared.Storage;
using Shared.Models;

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
                    true, e => e.Credentials);

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

        public Task<ResponseBaseModel> ChangePassword(ChangePasswordRequest request)
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
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
        // ~RemoteAuthenticationService()
        // {
        //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
