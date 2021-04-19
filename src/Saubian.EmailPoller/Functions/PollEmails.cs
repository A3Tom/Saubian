using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Saubian.Domain.Classes;
using Saubian.Domain.Models;
using Saubian.Domain.ViewModels;
using Saubian.EmailPoller.Helpers;
using Saubian.EmailPoller.Models;

namespace Saubian.EmailPoller.Functions
{
    public class PollEmails
    {
        private const string EMAIL_KEY_FUNCTION_NAME = nameof(GetEmailValues);
        private const string SB_MESSAGE_FUNCTION_NAME = nameof(SendMessageToServiceBus);

        private const string EMAIL_POLLER_APP_SETTING_KEY = "EmailPollerUrl";

        private static readonly TamsWeeProtoLogger _protoLog = new TamsWeeProtoLogger();

        private MailOptions _mailSettings = new MailOptions();


        [FunctionName("PollEmails")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req)
        {
            var request = JsonConvert.DeserializeObject<PollEmailRequest>(await req.Content.ReadAsStringAsync());

            await SetEmailKeys();

            var result = await ReadMessages(request.Mailbox, request.From, request.To);

            await SendServiceBusMessages(result);

            return new OkObjectResult(result);
        }

        [FunctionName("GetAllMailFolders")]
        public async Task<IActionResult> GetAllMailFolders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req)
        {
            await SetEmailKeys();

            var mailFolders = await GetAllMailFolders();

            return new OkObjectResult(mailFolders);
        }

        private async Task SendServiceBusMessages(IEnumerable<MessageDetail> messages)
        {
            var emailPollerUri = GetYerMawOnTheBlower.GetEnvironmentVariable(EMAIL_POLLER_APP_SETTING_KEY);

            var honkTheLot = new List<Task>();

            foreach (var message in messages)
            {
                honkTheLot.Add(GetYerMawOnTheBlower.Honk<string>(emailPollerUri, SB_MESSAGE_FUNCTION_NAME, message));
            }

            await Task.WhenAll(honkTheLot);
        }

        private async Task SetEmailKeys()
        {
            var emailPollerUri = GetYerMawOnTheBlower.GetEnvironmentVariable(EMAIL_POLLER_APP_SETTING_KEY);

            var keyQueryResponse = await GetYerMawOnTheBlower.Honk(emailPollerUri, EMAIL_KEY_FUNCTION_NAME);

            var emailKeys = JsonConvert.DeserializeObject<EmailKeyQueryResponse>(keyQueryResponse);

            _mailSettings.Account = emailKeys.Account;
            _mailSettings.ImapConfiguration = emailKeys.ImapComfig;
        }

        private async Task<IEnumerable<InboxDetail>> GetAllMailFolders()
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

        private async Task<IEnumerable<MessageDetail>> ReadMessages(string folder, int from, int count)
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
            var account = _mailSettings.Account;

            await client.ConnectAsync(imapConfig.Server, imapConfig.Port, SecureSocketOptions.SslOnConnect);
            client.Authenticate(account.Email, account.Password);
        }
    }
}
