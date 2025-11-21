

using Logic.Shared.ViewModels.Authentication;

namespace Web.App.Views.Authentication;

public partial class AuthenticationPage : ContentPage
{
	public AuthenticationPage(AuthenticationViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }
}