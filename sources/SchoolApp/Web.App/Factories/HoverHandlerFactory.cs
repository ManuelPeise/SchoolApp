namespace Web.App.Factories
{
    internal static class HoverHandlerFactory
    {
        internal static void Execute(string elementName, Shell? shell = null)
        {
            try
            {
                if (shell == null)
                {
                    return;
                }

                var registration = new HandlerRegistration();

                switch (elementName)
                {
                    case "Home":
                    {
                        var grid = shell.FindByName<Grid>("HomeGrid");       
                        registration.RegisterHoverEvents(grid);
                        break;
                    }
                    case "Admin":
                    {
                        var grid = shell.FindByName<Grid>("AdminHeaderGrid");
                        registration.RegisterHoverEvents(grid);
                        break;
                    }
                    case "DataSync":
                    {
                        var grid = shell.FindByName<Grid>("DataSyncHeaderGrid");
                        
                        registration.RegisterHoverEvents(grid);
                        break;
                    }
                    default:
                        throw new ArgumentException("Unknown element for hover handling.");
                }
            }
            catch
            {
                return;
            }
        }
    }
}
