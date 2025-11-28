using Data.Entities.Administration;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Authentication;
using Logic.Shared.Storage;
using Shared.Enums;
using Shared.Models;

namespace Logic.Authentication
{
    public class LocalAuthenticationService : IAuthenticationService
    {
        private readonly ILocalDatabaseAccessor _databaseAccessor;
        ICurrentUserService _currentUserService;
        private bool disposedValue;

        public LocalAuthenticationService(ILocalDatabaseAccessor databaseAccessor, ICurrentUserService currentUserService)
        {
            _databaseAccessor = databaseAccessor;
            _currentUserService = currentUserService;

            _currentUserService.SetCurrentUser();
        }

        public async Task<LoginResult> LoginAsync(LoginRequestModel model)
        {
            try
            {
                var user = await _databaseAccessor.UserRepository.Find(x => x.UserName.ToLower() == model.UserName.ToLower());

                if (user == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login failed, please check username!"
                    };
                }

                await _databaseAccessor.UserCredentialsRepository.GetEntityId(x => x.Id == user.CredentialsId);

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

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseBaseModel> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                if (_currentUserService.CurrentUser == null)
                {
                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Could not find user!"
                    };
                }

                var userEntity = await _databaseAccessor.UserRepository.Find(x => x.Id == _currentUserService.CurrentUser.Id, true, x => x.Credentials);

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
                    credentialsEntity.IsInSync = false;

                    await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser.UserName);

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
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Password validation failed.",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Info
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return new ResponseBaseModel
                {
                    Success = false,
                    Message = "Password validation failed!"
                };
            }
        }

        #region dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _databaseAccessor.Dispose();
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

        #endregion
    }
}
