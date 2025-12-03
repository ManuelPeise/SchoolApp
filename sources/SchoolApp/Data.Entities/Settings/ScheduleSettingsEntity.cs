using Shared.Enums;

namespace Data.Entities.Settings
{
    public class ScheduleSettingsEntity:AEntityBase
    {
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public ScheduleIntervalEnum Interval { get; set; }
    }
}
