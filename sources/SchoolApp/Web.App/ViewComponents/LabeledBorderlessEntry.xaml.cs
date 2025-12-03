using System.Windows.Input;

namespace Web.App.ViewComponents;

public partial class LabeledBorderlessEntry : ContentView
{


    public static readonly BindableProperty UnfocusedCommandProperty =
    BindableProperty.Create(nameof(UnfocusedCommand),
        typeof(ICommand),
        typeof(LabeledBorderlessEntry),
        default(ICommand));

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(LabeledBorderlessEntry), defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create(nameof(Label), typeof(string), typeof(LabeledBorderlessEntry), string.Empty);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LabeledBorderlessEntry), string.Empty);

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(int), typeof(LabeledBorderlessEntry), 12);

    public static readonly BindableProperty IsPasswordProperty =
       BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(LabeledBorderlessEntry), false);

    public ICommand UnfocusedCommand
    {
        get => (ICommand)GetValue(UnfocusedCommandProperty);
        set => SetValue(UnfocusedCommandProperty, value);
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public int FontSize
    {
        get => (int)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
    
    public LabeledBorderlessEntry()
	{
        InitializeComponent();

        ValueEntry.Unfocused += OnEntryFieldUnfocused;
    }

    private void OnEntryFieldUnfocused(object? sender, FocusEventArgs? e)
    {
        if (UnfocusedCommand?.CanExecute(null) ?? false)
        {
            UnfocusedCommand.Execute(null);
        }
            
    }
}