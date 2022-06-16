using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Saubian.Application.Queries.GetEmailMessages;
using System.Net.Http;
using System.Threading.Tasks;

namespace Saubian.EmailPoller.CyclonicActions
{
    public class EmailFunctions
    {
        private readonly IMediator _mediator;

        public EmailFunctions(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName(nameof(GetAllEmails))]
        public async Task<IActionResult> GetAllEmails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = null)] HttpRequestMessage req)
        {
            var result = await _mediator.Send(new GetEmailMessages.Request());

            return new OkObjectResult(result);
        }
    }
}