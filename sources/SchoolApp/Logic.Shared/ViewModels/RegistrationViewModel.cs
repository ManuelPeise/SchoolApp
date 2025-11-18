using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Logic.Shared.Interfaces;
using Logic.Shared.Models;

namespace Logic.Shared.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableUser _user = new();
        
        [ObservableProperty]
        private string _passwordReplication = string.Empty;

        [ObservableProperty]
        private bool _canRegister = false;

        public RegistrationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task RegisterUserAsync()
        {

        }

        [RelayCommand]
        private async Task NavigateToLoginAsync()
        {
            await _navigationService.NavigateToAsync("///login");
        }
    }
}
