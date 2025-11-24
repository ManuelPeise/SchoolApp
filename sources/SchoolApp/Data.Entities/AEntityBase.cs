using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public abstract class AEntityBase
    {
        [Key]
        public int Id { get; set; }
        public bool IsInSync { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
