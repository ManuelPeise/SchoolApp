namespace Web.App.Views.Sync;

public partial class ProfileDataSyncPage : ContentPage
{
	public ProfileDataSyncPage(ProfileDataSyncViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}