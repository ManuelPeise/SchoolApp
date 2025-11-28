using Data.Entities.Administration;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Shared.Enums;
using Shared.Models;

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

        public async Task<ResponseBaseModel> ChangeProfile(AppUserEntity entityToUpdate)
        {
            try
            {
                var existingEntity = await _databaseAccessor.UserRepository.GetByIdAsync(entityToUpdate.Id);

                if (existingEntity == null)
                {
                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Profile not found."
                    };
                }

                existingEntity = entityToUpdate;

                _databaseAccessor.UserRepository.Update(existingEntity);

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return new ResponseBaseModel
                {
                    Success = true,
                    Message = "Profile updated."
                };
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

                return new ResponseBaseModel
                {
                    Success = false,
                    Message = "Update profile local failed."
                };
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
