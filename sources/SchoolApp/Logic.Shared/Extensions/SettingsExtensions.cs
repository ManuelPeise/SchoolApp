using Data.Entities.Settings;
using Data.Entities.User;
using Logic.Shared.Models.Settings;
using Shared.Enums;

namespace Logic.Shared.Extensions
{
    public static class SettingsExtensions
    {
        public static ObservableSettings ToObservableSettings(this AppUserEntity? entity)
        {
            if (entity?.Settings == null)
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

            return new ObservableSettings
            {
                Id = entity.Settings.Id,
                Theme = entity.Settings.Theme,
                AutoSync = entity.Settings.AutoSync,
                ScheduleSettings = new ObservableScheduleSettings
                {
                    Id = entity.Settings.ScheduleSettings.Id,
                    Day = entity.Settings.ScheduleSettings.Day,
                    Hour = entity.Settings.ScheduleSettings.Hour,
                    Minute = entity.Settings.ScheduleSettings.Minute,
                    Interval = entity.Settings.ScheduleSettings.Interval
                }
            };
        }

        public static SettingsEntity? ToObservable(this ObservableSettings settings)
        {
            if (settings == null)
            {
                return null;
            }

            return new SettingsEntity
            {
                Id = settings.Id,
                Theme = settings.Theme,
                AutoSync = settings.AutoSync,
                ScheduleSettings = new ScheduleSettingsEntity
                {
                    Id = settings.ScheduleSettings.Id,
                    Day = settings.ScheduleSettings.Day,
                    Hour = settings.ScheduleSettings.Hour,
                    Minute = settings.ScheduleSettings.Minute,
                    Interval = settings.ScheduleSettings.Interval
                }
            };
        }
    }
}
