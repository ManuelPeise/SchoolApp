using Logic.Shared.Models;
using Shared.Enums;

namespace Logic.Shared.Helpers
{
    public static class FileImportHelper
    {
        internal static List<FileImportItem> GetFileImportItemModels()
        {
            return new List<FileImportItem>
            {
                new FileImportItem
                {
                    RequiredUserRole = UserRoleEnum.SystemAdmin,
                    FileType = FileTypeEnum.FamilyJsonFile,
                    Label = "Famile & Benutzer",
                    Description = "Familie oder Benutzer importieren."
                },
                new FileImportItem
                {
                    RequiredUserRole = UserRoleEnum.Admin,
                    FileType = FileTypeEnum.VocabularyJsonFile,
                    Label = "Vokabeln",
                    Description = "Vokabeldatensatz für Vokabeltraining importieren."
                }
            };
        }
    }
}
