using Logic.Shared.Interfaces;

namespace Web.App.Views.Authentication;

public partial class LoadingPage : ContentPage
{
    private readonly LoadingPageViewModel _vm;
    public LoadingPage(LoadingPageViewModel vm)
    {
        _vm = vm;
        InitializeComponent();
        BindingContext = _vm;
    }

    private void LoadingPageLoaded(object sender, EventArgs e)
    {
        _vm.LoadingPageLoaded();
    }
}