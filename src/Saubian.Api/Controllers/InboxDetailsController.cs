using System.Threading.Tasks;
using Saubian.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Saubian.Api.Controllers
{
    [ApiController]
	[Route("api/[controller]/[action]")]
    public class InboxDetailsController : ControllerBase
	{
		private readonly IInboxService _inboxService;

		public InboxDetailsController(IInboxService inboxService)
		{
			_inboxService = inboxService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllFolders()
		{
			var result = await _inboxService.GetAllMailFolders();

			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetFolderMailSubjectLines(string folder, int from = 0, int count = 10)
		{
			var result = await _inboxService.ReadMessages(folder, from, count);

			return Ok(result);
		}
	}
}