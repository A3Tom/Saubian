using System.Collections.Generic;

namespace Saubian.Domain.Models
{
    public class MailOptions 
    {
        public const string Section = "MailOptions";

        public IEnumerable<Account> Accounts { get; set; }
        public Account Account { get; set; }
        public ImapConfiguration ImapConfiguration { get; set; }
    }
}
