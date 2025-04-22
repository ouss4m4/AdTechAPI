using AdTechAPI.Enums;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;

namespace AdTechAPI.Helpers
{
    public static class DeviceHelper
    {
        public static string MapDeviceType(string detectedDevice)
        {
            return detectedDevice switch
            {
                "smartphone" => "Phone",
                "feature phone" => "Phone",
                "phablet" => "Phone",
                "tablet" => "Tablet",
                "desktop" => "Desktop",
                _ => "Desktop"
            };
        }

        public static int GetDeviceIdFromUserAgent(string userAgent)
        {

            var deviceDetector = new DeviceDetector(userAgent);
            deviceDetector.Parse();
            return (int)Enum.Parse<Platform>(MapDeviceType(deviceDetector.GetDeviceName()), true);

        }

    }
}
