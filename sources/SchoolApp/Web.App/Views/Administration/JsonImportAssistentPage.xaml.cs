namespace Web.App.Views.Administration;

public partial class JsonImportAssistentPage : ContentPage
{
	public JsonImportAssistentPage(JsonImportAssistentPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}