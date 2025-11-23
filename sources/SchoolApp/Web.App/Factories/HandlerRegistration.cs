namespace Web.App.Factories
{
    internal class HandlerRegistration
    {
        internal void RegisterHoverEvents(Grid grid)
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
                                if (grid != null)
                                    grid.BackgroundColor = Color.FromArgb("#262626");
                            });
                        };

                        fe.PointerExited += (_, e) =>
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                            if (grid != null)
                                    grid.BackgroundColor = Color.FromArgb("#1a1a1a");
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
