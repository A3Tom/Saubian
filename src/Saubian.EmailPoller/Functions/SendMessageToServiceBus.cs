using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Saubian.Service.Classes;

namespace Saubian.EmailPoller.Functions
{
    public class SendMessageToServiceBus
    {
        [FunctionName(nameof(SendMessageToServiceBus))]
        [return: ServiceBus("theloft", Connection = "ServiceBusConnection")]
        public async Task RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [ServiceBus("theloft", Connection = "ServiceBusConnection")] MessageSender messagesQueue)
        {
            Console.WriteLine("Well Look who it isnae. Yer about tae chant into the great unknown so fuckin lets. GOOOOOO");

            await MessageBusHandler.AmChantin(messagesQueue, req.Body);
        }
    }
}
