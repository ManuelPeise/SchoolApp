using CommunityToolkit.Mvvm.ComponentModel;

namespace Logic.Shared.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isLoading;

        protected void SetIsLoading(bool value)
        {
            IsLoading = value;
           
        }
    }
}
