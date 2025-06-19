namespace Epr.Reprocessor.Exporter.Facade.App.Constants;
public static class LogMessages
{
    public const string CreateRegistration = "Attempting to create registration application";
    public const string UpdateRegistrationTaskStatus = "Attempting to update registration site address and contact details";
    public const string UpdateRegistrationSiteAddress = "Attempting to update registration site address and contact details";
    public const string InvalidRequest = "Invalid Request";
    public const string  MaterialExemptionReferencesCreated = "Material Exemption References created successfully";
    public const string MaterialExemptionReferenceNotCreated = "Material Exemption Reference not created. Please check the request and try again.";

    public const string CreateExemptionReferences = "Attempting to create registration exemption references";
    public const string CreateRegistrationMaterial = "Attempting to create registration material";    
    public const string RegistrationMaterialAndExemptionReferencesNotCreated = "Registration Material and Exemption References not created";
    public const string UnExpectedError = "An unexpected error occurred.";
    public const string GetRegistrationByOrganisation = "Attempting to get registration of type {0} for organisation with ID {1}";
    public const string UpdateRegistrationMaterialPermits = "Attempting to update registration material permits with External ID {Id}";
    public const string UpdateRegistrationMaterialPermitCapacity = "Attempting to update registration material permit capacity with External ID {Id}";
    public const string GetMaterialsPermitTypes = "Attempting to get material permit types";

    public const string GetRegistrationOverview = "Attempting to get the overview for registration id {0}";
    public const string GetAllMaterials = "Attempting to retrieve a list of applicable materials";
    public const string GetAllRegistrationMaterials = "Attempting to retrieve all registration materials for registration {0}";
}
