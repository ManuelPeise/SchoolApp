using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models.Import;

namespace Logic.Import
{
    internal static class FileImportFactory
    {
        internal static AFileImporter Execute(FileImportModel model, IDbContextFactory dbContextFactory, ICurrentUserService currentUserService)
        {
            switch (model.FileType)
            {
                case FileTypeEnum.FamilyJsonFile:
                    return new FamilyFileImporter(model, dbContextFactory, currentUserService);
                case FileTypeEnum.VocabularyJsonFile:
                    return new VocabularyFileImporter(model, dbContextFactory, currentUserService);
                default: throw new ArgumentOutOfRangeException(nameof(model.FileType));
            }
        }
    }
}
