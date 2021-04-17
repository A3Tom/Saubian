using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Saubian.Domain.Models;
using Saubian.EmailPoller.Helpers;
using Saubian.EmailPoller.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Linq;

namespace Saubian.EmailPoller.Functions
{
    public class GetEmailValues
    {
        private const string KEYVAULT_FUNCTION_NAME = "GetKeyVaultValue";
        private const string EMAIL_POLLER_APP_SETTING_KEY = "EmailPollerUrl";

        [FunctionName(nameof(GetEmailValues))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req
            )
        {
            var emailPollerUri = GetYerMawOnTheBlower.GetEnvironmentVariable(EMAIL_POLLER_APP_SETTING_KEY);

            var taskList = new List<Task<string>>()
            {
                GetYerMawOnTheBlower.Honk<string>(emailPollerUri, KEYVAULT_FUNCTION_NAME, "EmailUser"),
                GetYerMawOnTheBlower.Honk<string>(emailPollerUri, KEYVAULT_FUNCTION_NAME, "EmailPassword"),
                GetYerMawOnTheBlower.Honk<string>(emailPollerUri, KEYVAULT_FUNCTION_NAME, "EmailServer"),
                GetYerMawOnTheBlower.Honk<string>(emailPollerUri, KEYVAULT_FUNCTION_NAME, "EmailPort")
            };

            var result = await Task.WhenAll(taskList);

            if (result.Any(x => string.IsNullOrEmpty(x)))
                return new NotFoundResult();

            // TODO : I hate this method of extracting data from an array. 
            // I need to improve that shit because am I fuck keepin this here
            var response = new EmailKeyQueryResponse()
            {
                Account = new Account()
                {
                    Email = result[0],
                    Password = result[1]
                },
                ImapComfig = new ImapConfiguration()
                {
                    Server = result[2],
                    Port = int.Parse(result[3])
                }
            };

            return new OkObjectResult(response);
        }
    }
}
