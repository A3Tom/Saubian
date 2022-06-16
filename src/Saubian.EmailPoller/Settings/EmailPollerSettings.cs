using Saubian.Domain.Interfaces.Settings;

namespace Saubian.EmailPoller.Settings
{
    public class EmailPollerSettings : IIMapAccountSettings
    {
        public string TargetEmailAccount { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}