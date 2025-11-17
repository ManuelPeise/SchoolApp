using CommunityToolkit.Mvvm.ComponentModel;

namespace Logic.Shared.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;

        protected bool SetBusy(bool value)
        {
            IsBusy = value;
            return value;
        }
    }
}
