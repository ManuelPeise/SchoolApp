using Shared.Enums;


namespace Shared.Models.Sync
{
    public class AppUserSyncModel: AsyncModelBase
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public bool IsActive { get; set; }
        public int? FamilyId { get; set; }
        public FamilySyncModel? Family { get; set; }
        public int CredentialsId { get; set; }
        public AppUserCredentialsSyncModel Credentials { get; set; } = new();
    }
}
