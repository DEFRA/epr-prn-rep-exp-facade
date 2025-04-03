using Epr.Reproccessor.Exporter.Facade.Api.Models;
using Epr.Reproccessor.Exporter.Facade.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reproccessor.Exporter.Facade.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaveAndContinueController (ISaveAndContinueService service, ILogger<SaveAndContinueController> logger) : Controller
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(SaveAndContinueRequest request)
        {
            try
            {
                await service.AddAsync(request);
                return Ok();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "SaveAndContinueController - Create: {request}: Recieved Unhandled exception", request);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetLastet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLatest(int registtrationId, string area)
        {
            try
            {
                var response = await service.GetLatestAsync(registtrationId, area);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SaveAndContinueController - GetLatest: registrationId:{registratioId} and area:{area}: Recieved Unhandled exception", registtrationId, area);
            }
            return BadRequest();
        }
    }
}
