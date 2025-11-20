using Data.Entities.LearnContent;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.User
{
    public class AppUserEntity: AEntityBase
    {
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Salt { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRoleEnum UserRole { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<UserLearnTopicEntity> UserLearnTopics { get; set; } = [];
        public int? FamilyId { get; set; }
        [ForeignKey(nameof(FamilyId))]
        public FamilyEntity? Family { get; set; }
    }
}
