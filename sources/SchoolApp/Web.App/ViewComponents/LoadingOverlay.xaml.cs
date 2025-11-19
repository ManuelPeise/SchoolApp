namespace Web.App.ViewComponents;

public partial class LoadingOverlay : ContentView
{
    public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(LoadingOverlay),
            false,
            propertyChanged: OnIsLoadingChanged);

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    private static void OnIsLoadingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (LoadingOverlay)bindable;
        bool isVisible = (bool)newValue;

        control.IsVisible = isVisible;

        if (isVisible)
        {
            control.FadeTo(1, 200, Easing.CubicIn);
        }
        else
        {
            control.FadeTo(0, 200, Easing.CubicOut);
        }
    }

    public LoadingOverlay()
    {
        InitializeComponent();
        IsVisible = false;
        Opacity = 0;
    }
}