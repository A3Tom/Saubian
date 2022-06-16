namespace Saubian.Domain.Interfaces.Settings
{
    public interface IIMapAccountSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
