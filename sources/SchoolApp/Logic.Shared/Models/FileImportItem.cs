using Shared.Enums;

namespace Logic.Shared.Models
{
    public class FileImportItem
    {
        public FileTypeEnum FileType { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AllowedFileType { get; set; } = ".json";
        public string? FileContent { get; set; } 
    }
}
