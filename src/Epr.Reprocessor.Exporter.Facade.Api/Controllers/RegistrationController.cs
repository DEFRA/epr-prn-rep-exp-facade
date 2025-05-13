using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Addresses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using static Microsoft.Identity.Web.Constants;

namespace Epr.Reprocessor.Exporter.Facade.Api.Controllers;

public class RegistrationController : ControllerBase
{
    private readonly ILogger<RegistrationController> _logger;
    private readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _httpClient; 
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly string _baseAddress;
    private readonly string[] _scopes;

    public RegistrationController(ILogger<RegistrationController> logger,
                                    HttpClient httpClient,
                                    ITokenAcquisition tokenAcquisition,
                                    IConfiguration configuration)
    {
        _logger = logger;
        _httpClient = httpClient;  
        _tokenAcquisition = tokenAcquisition;
        _baseAddress = configuration["EprServiceApi:BaseUrl"];
        _scopes = new[]
        {
            configuration["EprServiceApi:BaseUrl"],
        };
    }

    [HttpGet]
    [Route(ApiPaths.AddressLookUpByPostcode)]
    public async Task<AddressList?> AddressLookUpByPostcode(string postcodeToLookup)
    {
        try
        {
            await PrepareAuthenticatedClient();

            var response = await _httpClient.GetAsync($"/api/address-lookup?postcode={postcodeToLookup}");

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var addressResponse = await response.Content.ReadFromJsonAsync<AddressLookupResponse>();

            return new AddressList(addressResponse);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching addresses for postcode {postcodeToLookup}", postcodeToLookup);
            throw e;
        }
    }

    private async Task PrepareAuthenticatedClient()
    {
        _httpClient.BaseAddress ??= new Uri(_baseAddress);
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_scopes);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, accessToken);
    }
}
