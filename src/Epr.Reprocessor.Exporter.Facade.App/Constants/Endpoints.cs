using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Constants;

[ExcludeFromCodeCoverage]
public static class Endpoints
{
    public const string CreateRegistration = "api/v{0}/registrations";
    public const string RegistrationUpdateTaskStatus = "api/v{0}/registrations/{1}/taskStatus";
    public const string RegistrationUpdateSiteAddress = "api/v{0}/registrations/{1}/siteAddress";
    public const string GetRegistrationByOrganisation = "api/v{0}/registrations/{1}/organisations/{2}";
    public const string UpdateRegistration = "api/v{0}/registrations/{1}/update";
    public const string CreateExemptionReferences = "api/v{0}/registrationMaterials/createExemptionReferences";    
}

