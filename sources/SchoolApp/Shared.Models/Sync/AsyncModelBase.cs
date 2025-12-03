namespace Shared.Models.Sync
{
    public class AsyncModelBase
    {
        public int Id { get; set; }
        public bool IsInSync { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? LastSyncAt { get; set; }
    }
}
