using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Epr.Reprocessor.Exporter.Facade.App.Shared;
using Azure;
using Epr.Reprocessor.Exporter.Facade.Api.Extensions;
using System.Drawing.Printing;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers;

public class RegistrationController : ControllerBase
{
    private readonly ILogger<RegistrationController> _logger;
    private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

    public RegistrationController(ILogger<RegistrationController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route(ApiPaths.AddressLookUpByPostcode)]
    public async Task<IActionResult> AddressLookUpByPostcode( string postcodeToLookup )
    {
        try
        { 

            //var response = await  
            //if (response.IsSuccessStatusCode)
            //{ 
                // return Ok(paginatedResponse);
            //}

            return Ok("test");
            _logger.LogError("Failed to fetch pending applications");
            //return HandleError.HandleErrorWithStatusCode(response.StatusCode);
        }
        catch (Exception e)
        {
            //_logger.LogError(e, "Error fetching {PageSize} Pending applications for organisation {OrganisationName} on page {CurrentPage}", pageSize, organisationName, currentPage);
            return HandleError.Handle(e);
        }
    }
}
