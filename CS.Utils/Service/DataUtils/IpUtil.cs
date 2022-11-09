using System;
using System.Net;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class IpUtil
    {
        //public static readonly Regex RegexIp = new Regex(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
        //public static readonly Regex RegexHostName = new Regex(@"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$");

        public static bool IsValidIpAddress(string address)
        {
            return IPAddress.TryParse(address, out _);
        }

        public static bool IsValidHostName(string name)
        {
            return !string.IsNullOrEmpty(name) && Uri.CheckHostName(name) != UriHostNameType.Unknown;
        }

        public static bool IsValidAddress(string address)
        {
            return IsValidIpAddress(address) || IsValidHostName(address);
        }
    }
}
