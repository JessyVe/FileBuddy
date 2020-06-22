using System.Net.NetworkInformation;

namespace SharedResources.Services
{
    public static class MacAddressRetriever
    {
        private static readonly log4net.ILog Log
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the MAC address of the NIC with maximum speed.
        /// </summary>
        /// <returns>The MAC address.</returns>
        public static string GetMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                Log.Debug($"Found MAC Address: {nic.GetPhysicalAddress()} Type: {nic.NetworkInterfaceType}");

                var tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed && !string.IsNullOrEmpty(tempMac) && tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    Log.Debug($"New Max Speed = {nic.Speed} , MAC: {tempMac}");
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }
            return macAddress;
        }
    }
}
