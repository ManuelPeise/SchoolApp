using CommunityToolkit.Mvvm.ComponentModel;
using Shared.Enums;

namespace Logic.Shared.Models.Settings
{
    public partial class ObservableSettings: ObservableObject
    {
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private ThemeTypeEnum _theme;
        [ObservableProperty]
        private string _apiBaseUrl;
        [ObservableProperty]
        private int? _port;
        [ObservableProperty]
        private bool _autoSync;
        [ObservableProperty]
        private ObservableScheduleSettings _scheduleSettings = new();
    }

    public partial class ObservableScheduleSettings : ObservableObject
    {
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private int _day;
        [ObservableProperty]
        private int _hour;
        [ObservableProperty]
        private int _minute;
        [ObservableProperty]
        private ScheduleIntervalEnum _interval;
    }
}
