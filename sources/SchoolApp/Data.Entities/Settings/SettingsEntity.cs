using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Settings
{
    public class SettingsEntity:AEntityBase
    {
        public string Theme { get; set; } = string.Empty;
        public bool AutoSync { get; set; }
        public int ScheduleSettingsId { get; set; }
        [ForeignKey(nameof(ScheduleSettingsId))]
        public ScheduleSettingsEntity ScheduleSettings { get; set; } = new();
    }
}
