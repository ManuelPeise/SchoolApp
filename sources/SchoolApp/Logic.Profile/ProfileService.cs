using Data.ContextMysql;
using Data.Entities.Administration;
using Data.Entities.User;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models;

namespace Logic.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICurrentUserService _currentUserService;
        private bool disposedValue;

        public ProfileService(IDbContextFactory dbContextFactory, ICurrentUserService currentUserService)
        {
            _dbContextFactory = dbContextFactory;
            _currentUserService = currentUserService;
        }

        public async Task<ResponseBaseModel> ChangeProfileLocal(AppUserEntity entityToUpdate)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, _currentUserService);

            try
            {
                var existingEntity = await unitOfWork.UserRepository.GetByIdAsync(entityToUpdate.Id);

                if (existingEntity == null)
                {
                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Profile not found."
                    };
                }

                existingEntity = entityToUpdate;

                unitOfWork.UserRepository.Update(existingEntity);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite);

                return new ResponseBaseModel
                {
                    Success = true,
                    Message = "Profile updated."
                };
            }
            catch (Exception exception)
            {
                await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "Could not update profile local",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite);

                return new ResponseBaseModel
                {
                    Success = false,
                    Message = "Update profile local failed."
                };
            }

        }

        public async Task<ResponseBaseModel> ChangeProfileRemote(AppUserEntity entityToUpdate)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.MySql, _dbContextFactory, _currentUserService);

            try
            {
                var existingEntity = await unitOfWork.UserRepository.GetByIdAsync(entityToUpdate.Id);

                if (existingEntity == null)
                {
                    return new ResponseBaseModel
                    {
                        Success = false,
                        Message = "Profile not found."
                    };
                }

                existingEntity = entityToUpdate;

                unitOfWork.UserRepository.Update(existingEntity);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

                return new ResponseBaseModel
                {
                    Success = true,
                    Message = "Profile updated."
                };
            }
            catch (Exception exception)
            {
                await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "Could not update profile local",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.MySql);

                return new ResponseBaseModel
                {
                    Success = false,
                    Message = "Update profile local failed."
                };
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _currentUserService.Dispose();
                    _dbContextFactory.Dispose();
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
