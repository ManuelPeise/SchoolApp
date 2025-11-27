using Data.Entities.Administration;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models;

namespace Logic.Shared.Services
{
    public class SqLiteService : ISqLiteService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDbContextFactory _dbContextFactory;

        public SqLiteService(ICurrentUserService currentUserService, IDbContextFactory dbContextFactory)
        {
            _currentUserService = currentUserService;
            _dbContextFactory = dbContextFactory;

            _currentUserService.SetCurrentUser();
        }

        public async Task<(bool confirmed, string error)> CheckPassword(string password, int currentUserId, string currentUser)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, _currentUserService);

            try
            {
                var userEntity = await unitOfWork.UserRepository.GetByIdAsync((int)currentUserId, true, x => x.Credentials);

                if (userEntity == null)
                {
                    return (false, "Ups. da ist etwas schief gelaufen!");
                }

                var passwordHash = PasswordHelper.HashPassword(password, userEntity.Credentials.Salt);

                if (userEntity.Credentials.Password != passwordHash)
                {
                    return (false, "Überprüfe dein Passwort!");
                }

                return (true, string.Empty);
            }
            catch (Exception exception)
            {
                await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "Validate password failed.",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite, currentUser);

                return await Task.FromResult((false, "Passwort konnte nicht überprüft werden!"));
            }
        }

        public async Task HandleUpdateInSqLite(ChangePasswordRequest request, string currentUser, bool isInSync)
        {
            var unitOfWork = new ApplicationUnitOfWork(DatabaseProviderTypeEnum.SqLite, _dbContextFactory, _currentUserService);

            try
            {
                var userId = _currentUserService.GetCurrentUserId();

                var userEntity = await unitOfWork.UserRepository.Find(x => x.Id == userId, true, x => x.Credentials);

                if (userEntity == null
                    || userEntity.Credentials == null
                    || userEntity.Credentials.Password != PasswordHelper.HashPassword(request.Password, userEntity.Credentials.Salt))
                {
                    throw new Exception("Password update failed!");
                }

                var credentials = userEntity.Credentials;

                credentials.Password = PasswordHelper.HashPassword(request.NewPassword, credentials.Salt);

                unitOfWork.UserCredentialsRepository.Update(credentials);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite, currentUser);
            }
            catch (Exception exception)
            {
                await unitOfWork.LogRepository.AddAsync(new LogEntryEntity
                {
                    Message = "Update password failed.",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                }, null);

                await unitOfWork.SaveChangesAsync(DatabaseProviderTypeEnum.SqLite, currentUser);
            }
        }
    }
}
