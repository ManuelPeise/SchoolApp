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
                        var label = shell.FindByName<Label>("HomeLabel");
                        var icon = shell.FindByName<Image>("HomeIcon");
                        registration.RegisterHover(grid, label, icon);
                        break;
                    }
                    case "Admin":
                    {
                        var grid = shell.FindByName<Grid>("AdminHeaderGrid");
                        var label = shell.FindByName<Label>("AdminLabel");
                        var icon = shell.FindByName<Image>("AdminIcon");
                        registration.RegisterHover(grid, label, icon);
                        break;
                    }
                    case "SystemAdmin":
                    {
                        var grid = shell.FindByName<Grid>("SystemAdminHeaderGrid");
                        var label = shell.FindByName<Label>("SystemAdminLabel");
                        var icon = shell.FindByName<Image>("SystemAdminIcon");
                        registration.RegisterHover(grid, label, icon);
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
