using Data.Entities.Administration;
using Data.Entities.User;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Shared.Enums;

namespace Logic.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly ILocalDatabaseAccessor _databaseAccessor;
        private readonly ICurrentUserService _currentUserService;
        private bool disposedValue;

        public ProfileService(ILocalDatabaseAccessor databaseAccessor, ICurrentUserService currentUserService)
        {
            _databaseAccessor = databaseAccessor;
            _currentUserService = currentUserService;
        }

        public async Task<(bool success, string message)> UpdateProfile(AppUserEntity entityToUpdate)
        {
            try
            {
                var existingEntity = await _databaseAccessor.UserRepository.GetByIdAsync(entityToUpdate.Id);

                if (existingEntity == null)
                {
                    return (false, "Dein Profil wurde nicht gefunden.");
                }

                existingEntity = entityToUpdate;
                existingEntity.IsInSync = false;

                _databaseAccessor.UserRepository.Update(existingEntity);

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return (true, "Profil aktualisiert.");
            }
            catch (Exception exception)
            {
                await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Could not update profile local",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return (false, "Dein Profil konnte nicht aktualisiert werden.");
            }
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

        public async Task<bool> ChangePassword(string oldPassword, string newPassword, string currentUser)
        {
            try
            {
                var userId = _currentUserService.GetCurrentUserId();

                var userEntity = await _databaseAccessor.UserRepository.Find(x => x.Id == userId, true, x => x.Credentials);

                if (userEntity == null
                    || userEntity.Credentials == null
                    || userEntity.Credentials.Password != PasswordHelper.HashPassword(oldPassword, userEntity.Credentials.Salt))
                {
                    throw new Exception("Could not change password!");
                }

                var credentials = userEntity.Credentials;

                credentials.Password = PasswordHelper.HashPassword(newPassword, credentials.Salt);
                credentials.IsInSync = false;

                _databaseAccessor.UserCredentialsRepository.Update(credentials);

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return true;
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

                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _currentUserService.Dispose();
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
