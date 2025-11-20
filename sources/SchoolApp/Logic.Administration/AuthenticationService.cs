using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;

namespace Logic.Administration
{
    public class AuthenticationService : IAuthenticationService
    {
        private bool disposedValue;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IApplicationUnitOfWorkMySql _applicationUnitOfWorkMySql;
        private readonly IJwtTokenService _jwtTokenService;
        
        public AuthenticationService(
            IApplicationUnitOfWork applicationUnitOfWork, 
            IApplicationUnitOfWorkMySql applicationUnitOfWorkMySql,
            IJwtTokenService jwtTokenService)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _applicationUnitOfWorkMySql = applicationUnitOfWorkMySql;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResult> LoginAsync(LoginRequestModel model)
        {
            try
            {
                var user = await _applicationUnitOfWorkMySql.UserRepository.Find(x => x.Username.ToLower() == model.UserName.ToLower());

                if(user == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check username!"
                    };
                }

                var encriptedPassword = PasswordHelper.HashPassword(model.Password, user.Salt);

                if(encriptedPassword != user.Password)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check password!"
                    };
                }

                var tokenData = _jwtTokenService.GenerateTokens(user);

                user.RefreshToken = tokenData.RefreshToken;

                _applicationUnitOfWorkMySql.UserRepository.Update(user);

                await _applicationUnitOfWorkMySql.SaveChangesAsync();

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

        public async Task<LoginResult> LoginLocalAsync(LoginRequestModel model)
        {
            try
            {
                var user = await _applicationUnitOfWork.UserRepository.Find(x => x.Username.ToLower() == model.UserName.ToLower());

                if (user == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check username!"
                    };
                }

                var encriptedPassword = PasswordHelper.HashPassword(model.Password, user.Salt);

                if (encriptedPassword != user.Password)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check password!"
                    };
                }

                return new LoginResult
                {
                    Success = true,
                    
                    Message = "Login success!"
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
                    _applicationUnitOfWork.Dispose();
                    _applicationUnitOfWorkMySql.Dispose();
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
