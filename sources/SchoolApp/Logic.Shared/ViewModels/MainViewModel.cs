using CommunityToolkit.Mvvm.Input;
using Data.Entities.User;
using Logic.Shared.Interfaces;

namespace Logic.Shared.ViewModels
{
    public partial class MainViewModel: BaseViewModel
    {
        private readonly ICurrentUserService? _currentUserService;

        public MainViewModel(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        [RelayCommand]
        private void OnClick()
        {
            if (_currentUserService?.CurrentUser == null)
            {
                _currentUserService?.SetCurrentUser(new AppUserEntity { Username = "Test" });
            }
            else
            {
                _currentUserService?.SetCurrentUser(null);
            }
        }
    }
}
