using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MediatR;
using MimeKit;
using Saubian.Domain.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Saubian.Application.Queries.GetEmailMessages
{
    public class GetEmailMessages : IRequestHandler<GetEmailMessages.Request, bool>
    {
        private readonly IIMapAccountSettings _accountSettings;

        public GetEmailMessages(IIMapAccountSettings accountSettings)
        {
            _accountSettings = accountSettings;
        }

        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            var messages = await FetchAllMessages();

            foreach (var message in messages)
                Console.WriteLine($"New Message : \nFrom : {message.From} |\nSubject: {message.Subject}");

            return true;
        }

        public async Task<List<MimeMessage>> FetchAllMessages()
        {
            using ImapClient client = new ImapClient(new ProtocolLogger("C:\\Temp\\imap.log"));

            client.Connect(_accountSettings.Server, _accountSettings.Port, _accountSettings.UseSSL);
            client.Authenticate(_accountSettings.Username, _accountSettings.Password);

            // Get the number of messages in the inbox
            var folder = client.GetFolder(SpecialFolder.All);

            var messages = new List<MimeMessage>();

            await folder.OpenAsync(FolderAccess.ReadOnly);

            var results = folder.Search(SearchOptions.All, new SearchQuery());

            foreach (var id in results.UniqueIds)
            {
                var message = await folder.GetMessageAsync(id);
                messages.Add(message);
            }

            return messages;
        }

        public class Request : IRequest<bool>
        {
        }
    }
}


// 689f6a1a-7fee-4625-bb36-f02e0dff6fa6