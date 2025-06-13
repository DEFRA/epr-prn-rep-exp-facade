using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations
{
    [Route("api/v{version:apiVersion}/RegistrationMaterial")]
    [ApiVersion("1.0")]
    [ApiController]
    public class RegistrationMaterialController : ControllerBase
    {
        private readonly IRegistrationMaterialService _registrationMaterialService;
        private readonly ILogger<RegistrationMaterialController> _logger;

        public RegistrationMaterialController(IRegistrationMaterialService registrationMaterialService, ILogger<RegistrationMaterialController> logger)
        {
            _registrationMaterialService = registrationMaterialService ?? throw new ArgumentNullException(nameof(registrationMaterialService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("CreateExemptionReferences")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
        [SwaggerOperation(
            Summary = "creates a new exemption references",
            Description = "attempting to create new exemption references"
        )]
        public async Task<IActionResult> CreateExemptionReferences([FromBody] CreateExemptionReferencesDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning(LogMessages.InvalidRequest);
                return BadRequest(LogMessages.InvalidRequest);
            }

            _logger.LogInformation(LogMessages.CreateExemptionReferences);

            try
            {
                await _registrationMaterialService.CreateExemptionReferences(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
            }
        }
    }
}

