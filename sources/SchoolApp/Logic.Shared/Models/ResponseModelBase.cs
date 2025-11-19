namespace Logic.Shared.Models
{
    public class ResponseModelBase
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
