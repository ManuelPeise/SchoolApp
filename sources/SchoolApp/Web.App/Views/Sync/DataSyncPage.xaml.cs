namespace Web.App.Views.Sync;

public partial class DataSyncPage : ContentPage
{
	public DataSyncPage(DataSyncViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}