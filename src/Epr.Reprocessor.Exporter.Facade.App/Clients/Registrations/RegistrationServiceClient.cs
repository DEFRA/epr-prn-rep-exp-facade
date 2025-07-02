using System.Diagnostics.CodeAnalysis;
using System.Net;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;

public class RegistrationServiceClient(
HttpClient httpClient,
IOptions<PrnBackendServiceApiConfig> options,
ILogger<RegistrationServiceClient> logger)
: BaseHttpClient(httpClient), IRegistrationServiceClient
{
    private readonly PrnBackendServiceApiConfig _config = options.Value;

    [ExcludeFromCodeCoverage(Justification = "TODO: Unit tests to be added as part of create registration user story")]
    public async Task<CreateRegistrationResponseDto> CreateRegistrationAsync(CreateRegistrationDto request)
    {
        logger.LogInformation("CreateRegistrationAsync for ApplicationTypeId ID: {ApplicationTypeId}", request.ApplicationTypeId);

        var url = string.Format(Endpoints.Registration.CreateRegistration, _config.ApiVersion);

        return await this.PostAsync<CreateRegistrationDto, CreateRegistrationResponseDto>(url, request);
    }

    public async Task<RegistrationOverviewDto> GetRegistrationOverviewAsync(Guid registrationId)
    {
        logger.LogInformation("GetRegistrationOverviewAsync for Registration ID: {RegistrationId}", registrationId);
        var url = string.Format(Endpoints.Registration.RegistrationGetById, _config.ApiVersion, registrationId);

        return await this.GetAsync<RegistrationOverviewDto>(url);
    }
    public async Task<bool> UpdateRegistrationTaskStatusAsync(Guid registrationId, UpdateRegistrationTaskStatusDto request)
    {
        logger.LogInformation("UpdateRegistrationTaskStatusAsync for Registration ID: {RegistrationId}", registrationId);

        var url = string.Format(Endpoints.Registration.RegistrationUpdateTaskStatus, _config.ApiVersion, registrationId);

        return await this.PostAsync<UpdateRegistrationTaskStatusDto, bool>(url, request);
    }

    public async Task<bool> UpdateApplicantRegistrationTaskStatusAsync(Guid registrationMaterialId, UpdateRegistrationTaskStatusDto request)
    {
        logger.LogInformation("UpdateRegistrationTaskStatusAsync for registrationMaterialId: {registrationMaterialId}", registrationMaterialId);

        var url = string.Format(Endpoints.Registration.ApplicantRegistrationUpdateTaskStatus, _config.ApiVersion, registrationMaterialId);

        return await this.PostAsync<UpdateRegistrationTaskStatusDto, bool>(url, request);
    }

    public async Task<bool> UpdateSiteAddressAsync(Guid registrationId, UpdateRegistrationSiteAddressDto request)
    {
        logger.LogInformation("UpdateSiteAddressAsync for Registration ID: {RegistrationId}", registrationId);

        // e.g. api/v{0}/registrations/{1}/siteAddress
        var url = string.Format(Endpoints.Registration.RegistrationUpdateSiteAddress, _config.ApiVersion, registrationId);
        
        return await this.PostAsync<UpdateRegistrationSiteAddressDto, bool>(url, request);
    }

    public async Task<RegistrationDto?> GetRegistrationByOrganisationAsync(int applicationTypeId, Guid organisationId)
    {
        logger.LogInformation("Attempting to get existing registration for organisation with ID {OrganisationId}", organisationId);

        var url = string.Format(Endpoints.Registration.GetRegistrationByOrganisation, _config.ApiVersion, applicationTypeId, organisationId);

        try
        {
            return await GetAsync<RegistrationDto>(url);
        }
        catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while trying to get registration for organisation with ID {OrganisationId}", organisationId);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Guid registrationId, UpdateRegistrationDto request)
    {
        logger.LogInformation("Attempting to update an existing registration with ID {RegistrationId}", registrationId);

        var url = string.Format(Endpoints.Registration.UpdateRegistration, _config.ApiVersion, registrationId);

        return await PostAsync<UpdateRegistrationDto, bool>(url, request);
    }

    public async Task<IEnumerable<RegistrationsOverviewDto>> GetRegistrationsOverviewByOrgIdAsync(Guid organisationId)
    {
        logger.LogInformation("Attempting to get existing registrations overview for organisation with ID {OrganisationId}", organisationId);

        var url = string.Format(Endpoints.Registration.GetRegistrationsOverviewByOrgId, _config.ApiVersion, organisationId);

        try
        {
            return await GetAsync<IEnumerable<RegistrationsOverviewDto>>(url);
        }
        catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while trying to get registrations overview for organisation with ID {OrganisationId}", organisationId);
            throw;
        }
    }
}