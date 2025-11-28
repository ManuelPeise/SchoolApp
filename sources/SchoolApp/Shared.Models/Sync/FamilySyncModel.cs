namespace Shared.Models.Sync
{
    public class FamilySyncModel: AsyncModelBase
    {
        public string FamilyDisplayName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
    }
}
