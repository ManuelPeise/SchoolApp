using Data.Entities.LearnContent;
using Data.Entities.Settings;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.User
{
    public class AppUserEntity: AEntityBase
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public bool IsActive { get; set; }
        public List<UserLearnTopicEntity> UserLearnTopics { get; set; } = [];
        public int? FamilyId { get; set; }
        [ForeignKey(nameof(FamilyId))]
        public FamilyEntity? Family { get; set; }
        public int CredentialsId { get; set; }
        [ForeignKey(nameof(CredentialsId))]
        public AppUserCredentialsEntity Credentials { get; set; } = new();
        public int SettingsId { get; set; }
        [ForeignKey(nameof(SettingsId))]
        public SettingsEntity Settings { get; set; } = new();
    }
}
