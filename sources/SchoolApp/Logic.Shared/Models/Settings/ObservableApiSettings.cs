using CommunityToolkit.Mvvm.ComponentModel;

namespace Logic.Shared.Models.Settings
{
    public partial class ObservableApiSettings : ObservableObject
    {
        [ObservableProperty]
        private string _apiBaseUrl;
        [ObservableProperty]
        private int? _port;
    }
}
