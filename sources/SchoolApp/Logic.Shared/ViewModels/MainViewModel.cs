using CommunityToolkit.Mvvm.Input;
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
           
        }
    }
}
