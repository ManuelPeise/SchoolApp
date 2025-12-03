using Logic.Shared.Interfaces;
using Logic.Shared.Storage;
using Shared.Enums;
using Shared.Models.Import;

namespace Logic.Import
{
    internal static class FileImportFactory
    {
        internal static AFileImporter Execute(FileImportModel model, IRemoteDatabaseAccessor databaseAccessor)
        {
            switch (model.FileType)
            {
                case FileTypeEnum.FamilyJsonFile:
                    return new FamilyFileImporter(model, databaseAccessor);
                case FileTypeEnum.VocabularyJsonFile:
                    return new VocabularyFileImporter(model, databaseAccessor);
                default: throw new ArgumentOutOfRangeException(nameof(model.FileType));
            }
        }
    }
}
