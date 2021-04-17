using System.Collections.Generic;
using System.Threading.Tasks;
using Saubian.Domain.ViewModels;

namespace Saubian.Service.Interfaces
{
    public interface IInboxService
    {
        Task<IEnumerable<InboxDetail>> GetAllMailFolders();
        Task<IEnumerable<MessageDetail>> ReadMessages(string folder, int from, int count);
    }
}
