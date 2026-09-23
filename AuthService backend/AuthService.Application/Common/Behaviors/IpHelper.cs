using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Common.Behaviors
{
    public static class IpHelper
    {

        public static string GetIpRange(string? ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return "";

            if (ip.Contains(':'))
            {
                var parts = ip.Split(':');
                return string.Join(":", parts.Take(4));
            }

            var octets = ip.Split('.');
            if (octets.Length != 4) return ip;

            return $"{octets[0]}.{octets[1]}.{octets[2]}.*";
        }
    }
}
