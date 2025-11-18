using Logic.Shared.ViewModels;

namespace Web.App.Views.Authentication;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegistrationViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}