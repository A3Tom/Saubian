using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Saubian.Domain.Models;
using Saubian.EmailPoller.Models;
using Saubian.Service.Helpers;

namespace Saubian.EmailPoller.Functions
{
    public class GetEmailValues
    {
        private const string KEYVAULT_FUNCTION_NAME = "GetKeyVaultValue";
        private const string EMAIL_POLLER_APP_SETTING_KEY = "EmailPollerUrl";

        private const string EMAIL_USER_KEY = "EmailUser";
        private const string EMAIL_PASSWORD_KEY = "EmailPassword";
        private const string EMAIL_SERVER_KEY = "EmailServer";
        private const string EMAIL_PORT_KEY = "EmailPort";

        [FunctionName(nameof(GetEmailValues))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req
            )
        {
            var emailPollerUri = GetYerMawOnTheBlower.GetEnvironmentVariable(EMAIL_POLLER_APP_SETTING_KEY);

            var taskList = new List<Task<KeyValuePair<string, string>>>()
            {
                GetKeyVaultValueToTuple(emailPollerUri, EMAIL_USER_KEY),
                GetKeyVaultValueToTuple(emailPollerUri, EMAIL_PASSWORD_KEY),
                GetKeyVaultValueToTuple(emailPollerUri, EMAIL_SERVER_KEY),
                GetKeyVaultValueToTuple(emailPollerUri, EMAIL_PORT_KEY),
            };

            var result = await Task.WhenAll(taskList);

            if (result.Any(x => string.IsNullOrEmpty(x.Value)))
                return new NotFoundResult();

            var response = BuildQueryResponse(result);

            return new OkObjectResult(response);
        }

        private async Task<KeyValuePair<string, string>> GetKeyVaultValueToTuple(string keyvaultUrl, string key)
        {
            var function = await GetYerMawOnTheBlower.Honk<string>(keyvaultUrl, KEYVAULT_FUNCTION_NAME, key);

            return new KeyValuePair<string, string>(key, function); 
        }

        private EmailKeyQueryResponse BuildQueryResponse(KeyValuePair<string, string>[] keys)
        {
            return new EmailKeyQueryResponse()
            {
                Account = new Account()
                {
                    Email = keys.Single(x => x.Key == EMAIL_USER_KEY).Value,
                    Password = keys.Single(x => x.Key == EMAIL_PASSWORD_KEY).Value
                },
                ImapComfig = new ImapConfiguration()
                {
                    Server = keys.Single(x => x.Key == EMAIL_SERVER_KEY).Value,
                    Port = int.Parse(keys.Single(x => x.Key == EMAIL_PORT_KEY).Value)
                }
            };
        }
    }
}
