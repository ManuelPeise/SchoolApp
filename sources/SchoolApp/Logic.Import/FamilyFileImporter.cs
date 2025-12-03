using Data.Entities.Administration;
using Data.Entities.User;
using Logic.Import.Models;
using Logic.Shared;
using Logic.Shared.Storage;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Models;
using Shared.Models.Import;

namespace Logic.Import
{
    internal class FamilyFileImporter : AFileImporter
    {
        private readonly FileImportModel _fileImportModel;

        public FamilyFileImporter(FileImportModel fileImportModel, IRemoteDatabaseAccessor databaseAccessor)
            : base(databaseAccessor)
        {
            _fileImportModel = fileImportModel;
        }

        public override async Task<ResponseBaseModel> Execute()
        {
            try
            {
                if (string.IsNullOrEmpty(_fileImportModel.FileContent))
                {
                    await DatabaseAccessor.LogMessage(new LogEntryEntity
                    {
                        Message = $"Error: file: {_fileImportModel.FileName} is empty",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    });

                    await DatabaseAccessor.SaveChangesAsync();

                    return new ResponseBaseModel { Success = false, Message = $"Error: file: {_fileImportModel.FileName} is empty" };
                }

                var model = JsonConvert.DeserializeObject<FamilyImportModel>(_fileImportModel.FileContent);

                if (model == null)
                {
                    await DatabaseAccessor.LogMessage(new LogEntryEntity
                    {
                        Message = $"File: {_fileImportModel.FileName} could not be parsed.",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    });

                    await DatabaseAccessor.SaveChangesAsync();

                    return new ResponseBaseModel { Success = false, Message = $"Error, file: {_fileImportModel.FileName} could not be parsed." };
                }

                if (!model.IsValidModel())
                {
                    await DatabaseAccessor.LogMessage(new LogEntryEntity
                    {
                        Message = $"File: {_fileImportModel.FileName} could not be parsed.",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Info
                    });

                    await DatabaseAccessor.SaveChangesAsync();

                    return new ResponseBaseModel { Success = false, Message = $"File: {_fileImportModel.FileName} could not be parsed." };
                }

                var familyId = await DatabaseAccessor.FamilyRepository.GetEntityId(x => x.FamilyName.ToLower() == model.FamilyName.ToLower());

                var importTimeStamp = DateTime.UtcNow;

                if (familyId == null)
                {
                    var familyMembers = await GetFamilyMemberEntities(DatabaseAccessor, model.FamilyMembers, importTimeStamp);

                    var adminUser = familyMembers.FirstOrDefault(x => x.UserRole == UserRoleEnum.Admin);

                    if (adminUser == null)
                    {
                        await DatabaseAccessor.LogMessage(new LogEntryEntity
                        {
                            Message = $"Could not import family: {_fileImportModel.FileName} - admin user is not defined.",
                            ExceptionMessage = string.Empty,
                            Stacktrace = string.Empty,
                            LogLevel = LogLevelEnum.Info
                        });

                        await DatabaseAccessor.SaveChangesAsync();

                        return new ResponseBaseModel { Success = false, Message = $"Could not import family: {_fileImportModel.FileName} - admin user is not defined." };
                    }

                    var familyEntity = new FamilyEntity
                    {
                        FamilyName = $"{adminUser.FirstName}.{model.FamilyName}",
                        FamilyDisplayName = model.FamilyName,
                        FamilyMembers = familyMembers,
                        CreatedAt = importTimeStamp,
                        CreatedBy = "System"
                    };

                    await DatabaseAccessor.FamilyRepository.AddAsync(familyEntity, null);
                }

                await DatabaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = $"File: {_fileImportModel.FileName} imported with success.",
                    ExceptionMessage = string.Empty,
                    Stacktrace = string.Empty,
                    LogLevel = LogLevelEnum.Info
                });

                await DatabaseAccessor.SaveChangesAsync();

                return new ResponseBaseModel { Success = true, Message = $"Import {_fileImportModel.FileName} with success!" };

            }
            catch (Exception exception)
            {
                await DatabaseAccessor.LogMessage(new LogEntryEntity
                {
                    Message = $"Could not import file: {_fileImportModel.FileName} - family import failed.",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                await DatabaseAccessor.SaveChangesAsync();

                return new ResponseBaseModel { Success = false, Message = $"Could not import file: {_fileImportModel.FileName} - family import failed." };
            }
        }

        private async Task<List<AppUserEntity>> GetFamilyMemberEntities(IRemoteDatabaseAccessor databaseAccessor, List<FamilyMemberImportModel> familyMembers, DateTime timeStamp)
        {
            var members = new List<AppUserEntity>();

            foreach (var familyMember in familyMembers)
            {
                var userId = await databaseAccessor.UserRepository.GetEntityId(x =>
                    x.FirstName.ToLower() == familyMember.FirstName.ToLower() && x.LastName.ToLower() == familyMember.LastName.ToLower());

                if (userId == null)
                {
                    var salt = Guid.NewGuid().ToString();

                    members.Add(new AppUserEntity
                    {
                        FirstName = familyMember.FirstName,
                        LastName = familyMember.LastName,
                        DateOfBirth = familyMember.DateOfBirth,
                        UserRole = familyMember.UserRole,
                        IsInSync = true,
                        IsActive = familyMember.IsActive,
                        Credentials = new AppUserCredentialsEntity
                        {
                            Salt = salt,
                            Password = PasswordHelper.HashPassword(familyMember.Password, salt),
                            RefreshToken = string.Empty,
                            CreatedAt = timeStamp,
                            CreatedBy = "System"
                        },
                        CreatedAt = timeStamp,
                        CreatedBy = "System"
                    });
                }
            }

            return members;
        }
    }
}
