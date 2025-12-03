using Logic.Service.Android.Interfaces;
using Logic.Shared.Models;
using Logic.Shared.Storage;

namespace Logic.Service.Android
{
    public class UserServiceAndroid : IUserServiceAndroid
    {
        private int _currentUserId;
        private readonly ILocalDatabaseAccessor _localDatabaseAccessor;
        private ObservableUser? _currentUser;

        public ObservableUser? CurrentUser {  get { return _currentUser; } }
        
        
        public UserServiceAndroid(ILocalDatabaseAccessor localDatabaseAccessor)
        {
            _localDatabaseAccessor = localDatabaseAccessor;
        }

        public async Task<ObservableUser?> GetCurrentUser(string userIdString)
        {
            try
            {
                if (!int.TryParse(userIdString, out var userId))
                {
                    throw new ArgumentException(nameof(userIdString));
                }
            }
            catch(Exception exception)
            {

            }
        }

        public Task StoreUserData(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
