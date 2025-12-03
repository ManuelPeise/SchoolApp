namespace Web.App.Views.Authentication;

public partial class LogoutPage : ContentPage
{
	public LogoutPage(LogoutPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}