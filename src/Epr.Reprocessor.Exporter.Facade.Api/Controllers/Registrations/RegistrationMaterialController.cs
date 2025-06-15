using System.Net;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;

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

    [HttpPost("CreateRegistrationMaterial")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
    [SwaggerOperation(
        Summary = "creates a new registration material",
        Description = "attempting to create new registration material"
    )]
    public async Task<IActionResult> CreateRegistrationMaterial([FromBody] CreateRegistrationMaterialRequestDto requestDto)
    {
        if (requestDto == null)
        {
            _logger.LogWarning(LogMessages.InvalidRequest);
            return BadRequest(LogMessages.InvalidRequest);
        }

        _logger.LogInformation(LogMessages.CreateRegistrationMaterial);

        try
        {
            var registrationMaterialId = await _registrationMaterialService.CreateRegistrationMaterial(requestDto);
            
            return new CreatedResult(string.Empty, registrationMaterialId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
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

    [HttpGet("{registrationId:guid}/materials")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RegistrationMaterialDto>))]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(
        Summary = "gets existing registration materials associated with a registration.",
        Description = "attempting to get existing registration materials associated with a registration."
    )]
    public async Task<IActionResult> GetAllRegistrationMaterials([FromRoute]Guid registrationId)
    {
        _logger.LogInformation(LogMessages.GetAllRegistrationMaterials, registrationId);

        try
        {
            var materials = await _registrationMaterialService.GetAllRegistrationsMaterials(registrationId);
            return Ok(materials);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, LogMessages.UnExpectedError);
        }
    }
}