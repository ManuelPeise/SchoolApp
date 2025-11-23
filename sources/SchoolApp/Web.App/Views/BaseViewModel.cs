using CommunityToolkit.Mvvm.ComponentModel;

namespace Web.App.Views
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
