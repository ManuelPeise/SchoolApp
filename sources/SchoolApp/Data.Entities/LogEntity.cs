namespace Data.Entities
{
    public class LogEntity: AEntityBase
    {
        public string Message { get; set; } = string.Empty;
        public string ExeptionMessage { get; set; } = string.Empty;
        public string Stacktrace { get; set; } = string.Empty;
    }
}
