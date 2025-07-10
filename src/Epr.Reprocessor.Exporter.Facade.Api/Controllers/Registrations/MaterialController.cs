using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;

/// <summary>
/// Controller for managing materials in the reprocessor/exporter journey.
/// </summary>
[Route("api/v{version:apiVersion}/materials")]
[ApiVersion("1.0")]
[ApiController]
public class MaterialController : ControllerBase
{
    private readonly IMaterialService _materialService;
    private readonly ILogger<MaterialController> _logger;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="materialService">Provides a service to manage materials.</param>
    /// <param name="logger">A logger instance to log to.</param>
    public MaterialController(IMaterialService materialService, ILogger<MaterialController> logger)
    {
        ArgumentNullException.ThrowIfNull(materialService);
        ArgumentNullException.ThrowIfNull(logger);

        _materialService = materialService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all materials available for the reprocessor/exporter journey.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IEnumerable<AvailableMaterialDto>))]
    [SwaggerOperation(
        Summary = "Retrieve all materials.",
        Description = "Retrieves a list of materials that can be applied for during the reprocessor/exporter journey."
    )]
    public async Task<IActionResult> GetAllMaterials()
    {
        _logger.LogInformation(LogMessages.GetAllMaterials);

        var materials = await _materialService.GetAllMaterialsAsync();

        return Ok(materials);
    }
}