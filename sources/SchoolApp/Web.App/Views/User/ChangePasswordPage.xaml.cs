namespace Web.App.Views.User;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage(ChangePasswordViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }
}