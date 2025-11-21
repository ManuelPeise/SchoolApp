namespace Shared.Models
{
    public class ResponseBaseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
