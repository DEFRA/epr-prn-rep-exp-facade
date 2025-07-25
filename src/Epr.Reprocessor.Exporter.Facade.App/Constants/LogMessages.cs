﻿namespace Epr.Reprocessor.Exporter.Facade.App.Constants;
public static class LogMessages
{
    public const string CreateRegistration = "Attempting to create registration application";
    public const string UpdateRegistrationTaskStatus = "Attempting to update registration task status";
    public const string UpdateApplicationRegistrationTaskStatus = "Attempting to update application registration task status";
    public const string UpdateRegistrationSiteAddress = "Attempting to update registration site address and contact details";
    public const string InvalidRequest = "Invalid Request";
    public const string  MaterialExemptionReferencesCreated = "Material Exemption References created successfully";
    public const string MaterialExemptionReferenceNotCreated = "Material Exemption Reference not created. Please check the request and try again.";

    public const string CreateExemptionReferences = "Attempting to create registration exemption references";
    public const string CreateRegistrationMaterial = "Attempting to create registration material";    
    public const string RegistrationMaterialAndExemptionReferencesNotCreated = "Registration Material and Exemption References not created";
    public const string UnExpectedError = "An unexpected error occurred.";
    public const string GetRegistrationByOrganisation = "Attempting to get registration of type {ApplicationTypeId} for organisation with ID {OrganisationId}";
    public const string UpdateRegistrationMaterialPermits = "Attempting to update registration material permits with External ID {Id}";
    public const string UpdateRegistrationMaterialPermitCapacity = "Attempting to update registration material permit capacity with External ID {Id}";
	public const string UpdateIsMaterialRegistered = "Attempting to update the registration material IsMaterialRegistered flag.";
    public const string GetMaterialsPermitTypes = "Attempting to get material permit types";
    public const string UpdateMaximumWeight = "Attempting to update the maximum weight the site is capable of processing for the material {RegistrationMaterialId}.";

	public const string GetRegistrationOverview = "Attempting to get the overview for registration id {RegistrationId}";
    public const string GetAllMaterials = "Attempting to retrieve a list of applicable materials";
    public const string GetAllRegistrationMaterials = "Attempting to retrieve all registration materials for registration {RegistrationId}";
    public const string DeleteRegistrationMaterial = "Attempting to delete a registration material with ID {RegistrationMaterialId}";
    public const string GetRegistrationsOverviewByOrganisationId = "Fetching registration overview for Organisation ID: {OrganisationId}";
    public const string UpsertRegistrationMaterialContact = "Attempting to upsert a contact for registration material with ID {RegistrationMaterialId}";
    public const string UpsertRegistrationReprocessingDetails = "Attempting to upsert registration reprocessing details for registration material with ID {RegistrationMaterialId}";
    public const string GetCountries = "Attempting to get Countries";
    public const string UpdateMaterialNotReprocessingReason = "Attempting to update the reason for not reprocessing registration material with ID {RegistrationMaterialId}";
    public const string GetOverseasMaterialReprocessingSites = "Attempting to retrieve overseas reprocessing sites including corresponding interim sites for registrationMaterial {RegistrationMaterialId}";
    public const string SaveInterimSites = "Attempting to create or update interim sites.";
}
