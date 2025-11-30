using Data.Entities.Administration;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models.Settings;
using Logic.Shared.Storage;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Profile
{
    public class SettingsService: ISettingsService
    {
        private readonly ILocalDatabaseAccessor _databaseAccessor;
        private readonly ICurrentUserService _currentUserService;
        private bool disposedValue;

        public SettingsService(ILocalDatabaseAccessor databaseAccessor, ICurrentUserService currentUserService)
        {
            _databaseAccessor = databaseAccessor;
            _currentUserService = currentUserService;

            _currentUserService.SetCurrentUser();
        }

        public async Task<ObservableSettings> LoadSettings()
        {
            try
            {
                var userId = _currentUserService.GetCurrentUserId();

                if(userId == null)
                {
                    return GetDefaultSettings();
                }
                   
                var userEntity = await _databaseAccessor.UserRepository.GetByIdAsync(userId.Value, true, x => x.Settings, x => x.Settings.ScheduleSettings);

                if(userEntity == null || userEntity.Settings == null)
                {
                    return GetDefaultSettings();
                }

                return userEntity.ToObservableSettings();

            }
            catch(Exception exception)
            {
                 await _databaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = "Could not load settings",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await _databaseAccessor.SaveChangesAsync(_currentUserService.CurrentUser?.UserName);

                return GetDefaultSettings();
            }
        }

        private ObservableSettings GetDefaultSettings()
        {
            return new ObservableSettings
            {
                Id = 0,
                Theme = ThemeTypeEnum.Light,
                AutoSync = false,
                ScheduleSettings = new ObservableScheduleSettings
                {
                    Id = 0,
                    Day = 0,
                    Hour = 0,
                    Minute = 0,
                    Interval = ScheduleIntervalEnum.None
                }
            };
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _currentUserService.Dispose();
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
    }
}
