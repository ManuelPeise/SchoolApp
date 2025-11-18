using Data.Entities.LearnContent;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.User
{
    public class AppUserEntity: AEntityBase
    {
        public string Username { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Salt { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRoleEnum UserRole { get; set; }
        public int LernTopicId { get; set; }
        [ForeignKey("LernTopicId")]
        public ICollection<UserLearnTopic> UserLearnTopics { get; set; } = new List<UserLearnTopic>();
    }
}
