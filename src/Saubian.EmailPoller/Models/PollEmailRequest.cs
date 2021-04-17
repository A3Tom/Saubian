namespace Saubian.EmailPoller.Models
{
    public class PollEmailRequest
    {
        public string Mailbox { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}
