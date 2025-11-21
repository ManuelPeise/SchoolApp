using Shared.Enums;

namespace Logic.Import.Models
{
    public class FamilyMemberImportModel
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; } = string.Empty;
        public UserRoleEnum UserRole { get; set; }
        public bool IsActive { get; set; }
    }
}
