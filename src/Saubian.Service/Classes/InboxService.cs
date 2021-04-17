using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saubian.Domain.Models;
using Saubian.Domain.ViewModels;
using Saubian.Service.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Saubian.Service.Classes
{
    public class InboxService : IInboxService
    {
        private readonly IProtocolLogger _protoLog;
        private readonly MailOptions _mailSettings;

        public InboxService(IProtocolLogger protoLogger, IOptions<MailOptions> mailSettings)
        {
            _protoLog = protoLogger;
            _mailSettings = mailSettings.Value;
        }
		public async Task<IEnumerable<InboxDetail>> GetAllMailFolders()
		{
            var result = new List<InboxDetail>();

            using var client = new ImapClient(_protoLog);
            await ConnectToClient(client);

            var folders = await client.GetFoldersAsync(new FolderNamespace('.', ""), false);

            foreach (var folder in folders)
            {
                var whatIsThis = await folder.OpenAsync(FolderAccess.ReadOnly);
                result.Add(BuildInboxDetail(folder, whatIsThis));
            }

            client.Disconnect(true);

            return result;
        }

		public async Task<IEnumerable<MessageDetail>> ReadMessages(string folder, int from, int count)
		{
			var result = new List<MessageDetail>();

            using var client = new ImapClient(_protoLog);
            await ConnectToClient(client);

            var inbox = await client.GetFolderAsync(folder);
			inbox.Open(FolderAccess.ReadOnly);

			for (int i = from; i < count; i++)
			{
				var message = await inbox.GetMessageAsync(i);
				result.Add(new MessageDetail(message));
			}

			client.Disconnect(true);

            return result;
		}

        private InboxDetail BuildInboxDetail(IMailFolder folder, FolderAccess folderAccess)
        {
            return new InboxDetail()
            {
                Name = folder.FullName,
                MessageCount = folder.Count,
                UnreadCount = folder.Unread,
                Access = folderAccess
            };
        }

        private async Task ConnectToClient(ImapClient client)
        {
            var imapConfig = _mailSettings.ImapConfiguration;
            var account = _mailSettings.Accounts.First();

            await client.ConnectAsync(imapConfig.Server, imapConfig.Port, SecureSocketOptions.SslOnConnect);
            client.Authenticate(account.Email, account.Password);
        }
    }
}
