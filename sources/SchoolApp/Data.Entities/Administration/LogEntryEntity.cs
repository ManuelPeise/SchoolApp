using Shared.Enums;

namespace Data.Entities.Administration
{
    public class LogEntryEntity: AEntityBase
    {
        public string Message { get; set; } = string.Empty;
        public string ExceptionMessage { get; set; } = string.Empty;
        public string Stacktrace { get; set; } = string.Empty;
        public LogLevelEnum LogLevel { get; set; }
    }
}
