using Data.Entities.Administration;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models;

namespace Logic.Shared.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryBase<AppUserEntity> _userRepository;
        private readonly ILogService _logService;
        private bool disposedValue;
        private bool disposedValue1;

        public UserService(IRepositoryBase<AppUserEntity> userRepository, ILogService logService)
        {
            _userRepository = userRepository;
            _logService = logService;
        }

        public async Task CreateUser(UserModel user)
        {
            try
            {
                var existingUsers = _userRepository.GetBy(x => x.Username == user.Username && x.DateOfBirth == user.DateOfBirth);

                if (existingUsers.Any())
                {
                    throw new InvalidOperationException("Could not create user, user already exists!");
                }

                var salt = Guid.NewGuid().ToString();

                var entity = new AppUserEntity
                {
                    Username = user.Username,
                    DateOfBirth = user.DateOfBirth,
                    Salt = salt,
                    Password = !string.IsNullOrEmpty(user.Password) ? PasswordHelper.HashPassword(user.Password, salt) : null,
                    UserRole = user.UserRole,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System"
                };

                await _userRepository.AddAsync(entity, null);

                await _userRepository.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                await _logService.LogMessage(new LogEntryEntity
                {
                    Message = "Create new user failed!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });
            }
        }

        public async Task<AppUserEntity?> GetUser(string username)
        {
            try
            {
                var users = _userRepository.GetBy(x => x.Username == username);

                if (users.Count > 1)
                {
                    throw new Exception("Found more then one user.");
                }

                return await Task.FromResult(users.First());
            }
            catch (Exception exception)
            {
                await _logService.LogMessage(new LogEntryEntity
                {
                    Message = "Could not load user from database!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return null;
            }
        }

        public async Task<List<AppUserEntity>> GetUsers()
        {
            try
            {
                var users = _userRepository.GetAll();

                return await Task.FromResult(users);
            }
            catch (Exception exception)
            {
                await _logService.LogMessage(new LogEntryEntity
                {
                    Message = "Could not load users from database!",
                    ExceptionMessage = exception.Message,
                    Stacktrace = exception?.StackTrace ?? string.Empty,
                    LogLevel = LogLevelEnum.Error
                });

                return new List<AppUserEntity>();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue1)
            {
                if (disposing)
                {
                    _logService.Dispose();
                    _userRepository.Dispose();
                }

                disposedValue1 = true;
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
