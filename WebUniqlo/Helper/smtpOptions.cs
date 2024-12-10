using System.Net;

namespace WebUniqlo.Helper
{
    public class smtpOptions
    {
        public const string Name = "smtpName";
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
