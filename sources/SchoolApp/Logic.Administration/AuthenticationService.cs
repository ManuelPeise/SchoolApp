using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Shared.Enums;

namespace Logic.Administration
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtTokenService _jwtTokenService;
        private bool disposedValue;

        public AuthenticationService(
           IDbContextFactory dbContextFactory,
           ICurrentUserService currentUserService,
            IJwtTokenService jwtTokenService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResult> LoginAsync(LoginRequestModel model)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            try
            {

                var user = await unitOfWork.UserRepository.Find(x => x.Username.ToLower() == model.UserName.ToLower());

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

                var tokenData = _jwtTokenService.GenerateTokens(user);

                user.RefreshToken = tokenData.RefreshToken;
                user.IsInSync = true;

                unitOfWork.UserRepository.Update(user);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

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
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, _currentUserService);

            try
            {
                var user = await unitOfWork.UserRepository.Find(x => x.Username.ToLower() == model.UserName.ToLower());

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
                    _dbContextFactory.Dispose();
                    _currentUserService.Dispose();
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
