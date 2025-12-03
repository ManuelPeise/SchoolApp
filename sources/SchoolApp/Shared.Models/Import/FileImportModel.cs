using Shared.Enums;

namespace Shared.Models.Import
{
    public class FileImportModel
    {
        public FileTypeEnum FileType { get; set; }
        public string FileContent { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
