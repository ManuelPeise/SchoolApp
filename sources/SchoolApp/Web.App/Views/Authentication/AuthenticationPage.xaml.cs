using Logic.Shared.ViewModels;

namespace Web.App.Views.Authentication;

public partial class AuthenticationPage : ContentPage
{
	public AuthenticationPage(AuthenticationViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }
}