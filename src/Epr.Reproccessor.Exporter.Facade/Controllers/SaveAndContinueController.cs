using Epr.Reproccessor.Exporter.Facade.Api.Models;
using Epr.Reproccessor.Exporter.Facade.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reproccessor.Exporter.Facade.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveAndContinueController (ISaveAndContinueService service, ILogger<SaveAndContinueController> logger) : Controller
    {
        [HttpPost(Name = "Save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save(SaveAndContinueModel model)
        {
            try
            {
                await service.SaveAsync(model);
                return Ok();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "SaveAndContinueController - Save: {request}: Recieved Unhandled exception", model);
            }
            return BadRequest();
        }
    }
}
