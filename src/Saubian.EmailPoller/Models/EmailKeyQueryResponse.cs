using Saubian.Domain.Models;

namespace Saubian.EmailPoller.Models
{
    public class EmailKeyQueryResponse
    {
        public Account Account { get; set; }
        public ImapConfiguration ImapComfig { get; set; }
    }
}
