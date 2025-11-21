using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace Logic.Shared
{
    public static class CustomFileTypes
    {
        public static readonly FilePickerFileType Json = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
            { DevicePlatform.Android, new[] { "application/json" } }, // MIME type
            { DevicePlatform.WinUI, new[] { ".json" } },             // Windows extension
            { DevicePlatform.MacCatalyst, new[] { "json" } },        // Mac
            { DevicePlatform.iOS, new[] { "public.json" } }          // iOS
            });
    }
}
