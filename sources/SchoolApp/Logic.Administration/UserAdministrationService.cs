using Data.Entities.Administration;
using Logic.Shared;
using Logic.Shared.Extensions;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;
using Logic.Shared.Services;
using Shared.Enums;
using Shared.Models;

namespace Logic.Administration
{
    public class UserAdministrationService : IUserAdministrationService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDbSycronisationService _dbSycronisationService;
        private bool disposedValue;

        public UserAdministrationService(
            IDbContextFactory dbContextFactory,
            ICurrentUserService currentUserService,
            IDbSycronisationService dbSycronisationService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
            _dbSycronisationService = dbSycronisationService;
        }


        public async Task<ResponseBaseModel> RegisterUser(UserRegistrationRequestModel model)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            try
            {
                if (model.User == null)
                {
                    await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = "Could not register user, user is null",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, null);

                    await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Could not register user, user is null"
                    };
                }

                var entity = model.User.ToEntity();

                entity.Credentials.Salt = Guid.NewGuid().ToString();
                entity.Credentials.Password = PasswordHelper.HashPassword(entity.Credentials.Password, entity.Credentials.Salt);

                await unitOfWork.UserRepository
                    .AddAsync(entity, x => x.UserName == model.User.UserName);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

                var userId = await unitOfWork.UserRepository
                    .GetEntityId(x => x.UserName == model.User.UserName && x.DateOfBirth == entity.DateOfBirth);

                if (userId == null)
                {
                    await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                    {
                        Message = "Could not get user from database",
                        ExceptionMessage = string.Empty,
                        Stacktrace = string.Empty,
                        LogLevel = LogLevelEnum.Error
                    }, null);

                    await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Could not find user in from database"
                    };
                }

                await _dbSycronisationService.CreateUserRelatedMySqlTableEntries((int)userId);

                return new ResponseBaseModel
                {
                    Success = true,
                    Message = "Registration successful!"
                };
            }
            catch (Exception exception)
            {
                await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "User registration failed",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

                return new ResponseBaseModel
                {
                    Success = false,
                    Message = "User registration failed."
                }; ;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContextFactory.Dispose();
                    _currentUserService.Dispose();
                    _dbSycronisationService.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
