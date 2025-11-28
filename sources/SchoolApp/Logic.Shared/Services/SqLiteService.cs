using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Shared.Enums;
using Shared.Models;

namespace Logic.Shared.Services
{
    public class SqLiteService : ISqLiteService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILocalDatabaseAccessor _databaseAccessor;

        public SqLiteService(ICurrentUserService currentUserService, ILocalDatabaseAccessor databaseAccessor)
        {
            _currentUserService = currentUserService;
           _databaseAccessor = databaseAccessor;

            _currentUserService.SetCurrentUser();
        }

        public async Task<(bool confirmed, string error)> CheckPassword(string password, int currentUserId, string currentUser)
        {
            try
            {
                var userEntity = await _databaseAccessor.UserRepository.GetByIdAsync((int)currentUserId, true, x => x.Credentials);

                if (userEntity == null)
                {
                    return (false, "Ups. da ist etwas schief gelaufen!");
                }

                var passwordHash = PasswordHelper.HashPassword(password, userEntity.Credentials.Salt);

                if (userEntity.Credentials.Password != passwordHash)
                {
                    return (false, "Überprüfe dein Passwort!");
                }

                return (true, string.Empty);
            }
            catch (Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Validate password failed.",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return await Task.FromResult((false, "Passwort konnte nicht überprüft werden!"));
            }
        }

        public async Task HandleUpdateInSqLite(ChangePasswordRequest request, string currentUser, bool isInSync)
        {
            try
            {
                var userId = _currentUserService.GetCurrentUserId();

                var userEntity = await _databaseAccessor.UserRepository.Find(x => x.Id == userId, true, x => x.Credentials);

                if (userEntity == null
                    || userEntity.Credentials == null
                    || userEntity.Credentials.Password != PasswordHelper.HashPassword(request.Password, userEntity.Credentials.Salt))
                {
                    throw new Exception("Password update failed!");
                }

                var credentials = userEntity.Credentials;

                credentials.Password = PasswordHelper.HashPassword(request.NewPassword, credentials.Salt);

                _databaseAccessor.UserCredentialsRepository.Update(credentials);

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);
            }
            catch (Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Update password failed.",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);
            }
        }
    }
}
