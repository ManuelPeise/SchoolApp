namespace Logic.Import.Models
{
    public class FamilyImportModel
    {
        public int Id { get; set; }
        public string FamilyName { get; set; } = string.Empty;
        public List<FamilyMemberImportModel> FamilyMembers { get; set; } = [];

        public bool IsValidModel()
        {
            return !string.IsNullOrEmpty(FamilyName) && FamilyMembers.Any();
        }
    }
}
