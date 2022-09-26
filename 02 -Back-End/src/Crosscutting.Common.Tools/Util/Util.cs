using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Globalization;

namespace Crosscutting.Common.Tools.Util
{
    public static class Util
    {
        public static Dictionary<string, string> CreateExceptionRecord(Exception e)
        {
            return new Dictionary<string, string>
            {
                { "exceptionMsg", e.Message},
                { "Date" , DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}
            };
        }
        public static string GetIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        public static string GetTextFromPdf(string urlFile)
        {
            var uri = urlFile;
            WebClient wc = new WebClient();
            var x = wc.DownloadData(uri);
            string reply = Convert.ToBase64String(x);
            return reply;
        }
        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string UppercaseMonth(string date)
        {
            var dateArray = date.Split('/');
            var month = dateArray[1];

            var monthFinal = char.ToUpper(month[0]) + month.Substring(1);
            var dateFinal = dateArray[0].ToString() + " - " + monthFinal + " - " + dateArray[2].ToString();

            // Return char and concat substring.
            return dateFinal;
        }
    }
}
