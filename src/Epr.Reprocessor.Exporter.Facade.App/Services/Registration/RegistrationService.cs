﻿using System.Diagnostics.CodeAnalysis;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;

namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public class RegistrationService(IRegistrationServiceClient registrationServiceClient) : IRegistrationService
{
    [ExcludeFromCodeCoverage(Justification = "TODO: Unit tests to be added as part of create registration user story")]
    public async Task<int> CreateRegistrationAsync(CreateRegistrationDto dto)
    {
        return await registrationServiceClient.CreateRegistrationAsync(dto);
    }

    public async Task<bool> UpdateSiteAddressAsync(int registrationId, UpdateRegistrationSiteAddressDto dto)
    {
        return await registrationServiceClient.UpdateSiteAddressAsync(registrationId, dto);
    }

    public async Task<bool> UpdateRegistrationTaskStatusAsync(int registrationId, UpdateRegistrationTaskStatusDto dto)
    {
        return await registrationServiceClient.UpdateRegistrationTaskStatusAsync(registrationId, dto);
    }
}
