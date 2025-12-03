using System.Windows.Input;

namespace Web.App.ViewComponents;

public partial class LabeledSwitch : ContentView
{
    public static readonly BindableProperty LabelProperty =
       BindableProperty.Create(nameof(Label), typeof(string), typeof(LabeledSwitch), default(string));

    public string? Label
    {
        get => (string?)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public static readonly BindableProperty CheckedProperty =
      BindableProperty.Create(nameof(Checked), typeof(bool), typeof(LabeledSwitch), false, propertyChanged: OnCheckedChanged);

    public bool Checked
    {
        get => (bool)GetValue(CheckedProperty);
        set => SetValue(CheckedProperty, value);
    }

    public event EventHandler<bool>? CheckedChanged;

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(LabeledSwitch), null);

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }


    public LabeledSwitch()
    {
        InitializeComponent();
    }

    private static void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is LabeledSwitch control)
        {
            control.CheckedChanged?.Invoke(control, (bool)newValue);

            // Execute associated command if provided (no parameter)
            var cmd = control.Command;
            if (cmd != null && cmd.CanExecute(null))
            {
                cmd.Execute(null);
            }
        }
    }
}
