using CommunityToolkit.Mvvm.ComponentModel;

namespace Logic.Shared.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isLoading;
        [ObservableProperty]
        private string? _title;

        protected void SetIsLoading(bool value)
        {
            IsLoading = value;
           
        }
    }
}
