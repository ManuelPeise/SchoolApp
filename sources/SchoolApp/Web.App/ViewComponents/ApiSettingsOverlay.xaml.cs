using Logic.Shared.Models.Settings;
using System.Windows.Input;


namespace Web.App.ViewComponents;

public partial class ApiSettingsOverlay : ContentView
{
    public static readonly BindableProperty ApiSettingsProperty =
        BindableProperty.Create(nameof(ApiSettings), typeof(ObservableApiSettings), typeof(ApiSettingsOverlay), default(ObservableApiSettings), BindingMode.TwoWay);

    public ObservableApiSettings? ApiSettings
    {
        get => (ObservableApiSettings?)GetValue(ApiSettingsProperty);
        set => SetValue(ApiSettingsProperty, value);
    }

    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(ApiSettingsOverlay), null);

    public ICommand? SaveCommand
    {
        get => (ICommand?)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public ApiSettingsOverlay()
    {
        InitializeComponent();
    }
}
