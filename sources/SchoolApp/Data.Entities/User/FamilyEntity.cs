namespace Data.Entities.User
{
    public class FamilyEntity: AEntityBase
    {
        public string FamilyDisplayName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public ICollection<AppUserEntity> FamilyMembers { get; set; } = [];
    }
}
