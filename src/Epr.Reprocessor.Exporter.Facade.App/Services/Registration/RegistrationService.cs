﻿using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationService(IRegistrationServiceClient registrationServiceClient) : IRegistrationService
{
    public async Task<CreateRegistrationResponseDto> CreateRegistrationAsync(CreateRegistrationDto dto)
    {
        return await registrationServiceClient.CreateRegistrationAsync(dto);
    }

    public async Task<bool> UpdateSiteAddressAsync(Guid registrationId, UpdateRegistrationSiteAddressDto dto)
    {
        return await registrationServiceClient.UpdateSiteAddressAsync(registrationId, dto);
    }

    public Task<RegistrationDto?> GetRegistrationByOrganisationAsync(int applicationTypeId, Guid organisationId)
    {
        return registrationServiceClient.GetRegistrationByOrganisationAsync(applicationTypeId, organisationId);
    }

    public async Task<bool> UpdateAsync(Guid registrationId, UpdateRegistrationDto request)
    {
        return await registrationServiceClient.UpdateAsync(registrationId, request);
    }

    public async Task<bool> UpdateRegistrationTaskStatusAsync(Guid registrationId, UpdateRegistrationTaskStatusDto dto)
    {
        return await registrationServiceClient.UpdateRegistrationTaskStatusAsync(registrationId, dto);
    }
    public async Task<bool> UpdateApplicationRegistrationTaskStatusAsync(Guid registrationMaterialId, UpdateRegistrationTaskStatusDto dto)
    {
        return await registrationServiceClient.UpdateApplicationRegistrationTaskStatusAsync(registrationMaterialId, dto);
    }

    public async Task<RegistrationOverviewDto> GetRegistrationOverviewAsync(Guid registrationId)
    {
        return await registrationServiceClient.GetRegistrationOverviewAsync(registrationId);
    }

    public Task<IEnumerable<RegistrationsOverviewDto>> GetRegistrationsOverviewByOrgIdAsync(Guid organisationId)
    {
        return registrationServiceClient.GetRegistrationsOverviewByOrgIdAsync(organisationId);
    }
}
