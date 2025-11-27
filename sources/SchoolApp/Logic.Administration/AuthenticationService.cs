using Data.Entities.Administration;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;


namespace Logic.Administration
{
    public class AuthenticationService : ALogicBase, IAuthenticationService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtTokenService _jwtTokenService;
        private bool disposedValue;

        public AuthenticationService(
           IDbContextFactory dbContextFactory,
           ICurrentUserService currentUserService,
            IJwtTokenService jwtTokenService,
            IHttpContextAccessor httpContext) : base(httpContext)
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
                var user = await unitOfWork.UserRepository.Find(
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

                unitOfWork.UserCredentialsRepository.Update(user.Credentials);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql, CurrentUser.UserName ?? "System");

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
                var user = await unitOfWork.UserRepository.Find(x => x.UserName.ToLower() == model.UserName.ToLower());

                if (user == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check username!"
                    };
                }

                await unitOfWork.UserCredentialsRepository.GetEntityId(x => x.Id == user.CredentialsId);

                var encriptedPassword = PasswordHelper.HashPassword(model.Password, user.Credentials.Salt);

                if (encriptedPassword != user.Credentials.Password)
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

        public async Task<ResponseBaseModel> ChangePassword(ChangePasswordRequest request)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            try
            {
                if(CurrentUser == null)
                {
                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Could not find user!"
                    };
                }

                var userEntity = await unitOfWork.UserRepository.Find(x => x.Id == CurrentUser.UserId, true, x => x.Credentials);

                if (userEntity == null || userEntity.Credentials == null)
                {
                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Could not find User in database"
                    };
                }

                var passwordHash = PasswordHelper.HashPassword(request.Password, userEntity.Credentials.Salt);

                if (userEntity.Credentials.Password == request.Password)
                {
                    passwordHash = PasswordHelper.HashPassword(request.NewPassword, userEntity.Credentials.Salt);

                    var credentialsEntity = userEntity.Credentials;

                    credentialsEntity.Password = passwordHash;

                    await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql, CurrentUser.UserName);

                    return new ResponseBaseModel
                    {
                        Success = true,
                        Message = "Password changed!"
                    };
                }

                return new ResponseBaseModel
                {
                    Success = false,
                    Message = "Incorrect password!"
                };
            }
            catch (Exception exception)
            {
                await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "Password validation failed.",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Info
                }, null);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql, CurrentUser.UserName);

                return new ResponseBaseModel
                {
                    Success = false,
                    Message = "Password validation failed!"
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
