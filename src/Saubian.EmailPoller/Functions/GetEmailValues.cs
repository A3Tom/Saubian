using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Saubian.Domain.Models;
using Saubian.EmailPoller.Helpers;
using Saubian.EmailPoller.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Saubian.EmailPoller.Functions
{
    public class GetEmailValues
    {
        private const string KEYVAULT_FUNCTION_NAME = "GetKeyvaultValue";

        [FunctionName(nameof(GetEmailValues))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req
            )
        {
            var ringRing = new GetYerMawOnTheBlower();

            var taskList = new List<Task<string>>()
            {
                ringRing.Honk(KEYVAULT_FUNCTION_NAME, "EmailUser"),
                ringRing.Honk(KEYVAULT_FUNCTION_NAME, "EmailPassword"),
                ringRing.Honk(KEYVAULT_FUNCTION_NAME, "EmailServer"),
                ringRing.Honk(KEYVAULT_FUNCTION_NAME, "EmailPort")
            };

            var result = await Task.WhenAll(taskList);

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
