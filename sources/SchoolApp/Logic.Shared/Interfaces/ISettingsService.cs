using Logic.Shared.Models.Settings;

namespace Logic.Shared.Interfaces
{
    public interface ISettingsService: IDisposable
    {
        Task<ObservableSettings> LoadSettings();
    }
}
