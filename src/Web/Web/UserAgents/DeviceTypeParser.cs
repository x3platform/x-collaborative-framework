namespace X3Platform.Web.UserAgents
{
    public sealed class DeviceTypeParser
    {
        /// <summary>½âÎö</summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static DeviceType Parse(ClientInfo client)
        {
            var device = client.Device.Family;
            var userAgent = client.UserAgent.Family;
            var os = client.OS.Family;

            if (device == "iPad" || device == "Nexus 7")
            {
                return DeviceType.Tablet;
            }

            if (device == "iPhone")
            {
                return DeviceType.Mobile;
            }

            if (userAgent == "Mobile Safari" || userAgent == "Chrome Mobile" || userAgent == "IE Mobile")
            {
                return DeviceType.Mobile;
            }

            if (userAgent == "Safari" || userAgent == "Chrome" || userAgent == "IE")
            {
                return DeviceType.PC;
            }

            return DeviceType.Other;
        }
    }
}
