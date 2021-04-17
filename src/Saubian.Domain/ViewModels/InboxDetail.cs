using MailKit;

namespace Saubian.Domain.ViewModels
{
    public class InboxDetail
    {
        public InboxDetail()
        {

        }

        public string Name { get; set; }
        public FolderAccess Access { get; set; }
        public int MessageCount { get; set; }
        public int UnreadCount { get; set; }
    }
}
