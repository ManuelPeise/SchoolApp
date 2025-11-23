using Shared.Enums;

namespace Shared.Models.UiModels
{
    public class ShellItemModel
    {
        public string Title { get; set; } = string.Empty;
        public SectionTypeEnum SectionType { get; set; }
        public List<UserRoleEnum> UserRoles { get; set; } = [];
        public bool IsEnabled { get; set; }
        public string Route { get; set; } = string.Empty;
    }
}
