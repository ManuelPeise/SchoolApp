namespace Web.App.Factories
{
    internal class HandlerRegistration
    {
        internal void RegisterHover(Grid grid, Label label, Image icon)
        {
            if (grid == null)
                return;

            // When the handler is ready, attach to the native platform view events (Windows only)
            grid.HandlerChanged += (sender, args) =>
            {
#if WINDOWS
                try
                {
                    var platformView = grid?.Handler?.PlatformView;
                    if (platformView is Microsoft.UI.Xaml.FrameworkElement fe)
                    {
                        // Use lambdas capturing the MAUI controls so handlers can update the correct elements
                        fe.PointerEntered += (_, e) =>
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                if (label != null)
                                    label.TextColor = Colors.LightGray;
                                if (icon != null)
                                    icon.Opacity = 0.7;
                            });
                        };

                        fe.PointerExited += (_, e) =>
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                if (label != null)
                                    label.TextColor = Colors.White;
                                if (icon != null)
                                    icon.Opacity = 1.0;
                            });
                        };
                    }
                }
                catch
                {
                    // ignore platform-specific attachment failures
                }
#endif
            };
        }
    }
}
