using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Constants;

/// <summary>
/// Defines the endpoints for calls to the backend API.
/// </summary>
[ExcludeFromCodeCoverage]
public static class Endpoints
{
    /// <summary>
    /// Defines the endpoints for the registration service.
    /// </summary>
    public static class Registration
    {
        public const string CreateRegistration = "api/v{0}/registrations";
        public const string RegistrationUpdateTaskStatus = "api/v{0}/registrations/{1}/taskStatus";
        public const string RegistrationUpdateSiteAddress = "api/v{0}/registrations/{1}/siteAddress";
        public const string GetRegistrationByOrganisation = "api/v{0}/registrations/{1}/organisations/{2}";
        public const string UpdateRegistration = "api/v{0}/registrations/{1}/update";
        public const string AccreditationGetOrCreate = "api/v{0}/accreditation/{1}/{2}/{3}";
        public const string AccreditationGet = "api/v{0}/accreditation/{1}";
        public const string AccreditationPost = "api/v{0}/accreditation";
        public const string AccreditationPrnIssueAuthGet = "api/v{0}/accreditationPRNIssueAuth/{1}";
        public const string AccreditationPrnIssueAuthPost = "api/v{0}/accreditationPRNIssueAuth/{1}";
        public const string RegistrationGetById = "api/v{0}/registrations/{1}";
    }

    /// <summary>
    /// Defines the endpoints for the registration material service.
    /// </summary>
    public static class RegistrationMaterial
    {
        public const string CreateExemptionReferences = "api/v{0}/registrationMaterials/{1}/createExemptionReferences";
        public const string CreateRegistrationMaterial = "api/v{0}/registrationMaterials/create";
        public const string GetAllRegistrationMaterials = "api/v{0}/registrations/{1}/materials";
        public const string UpdateRegistrationMaterialPermits = "api/v{0}/registrationMaterials/{1}/permits";
        public const string GetMaterialsPermitTypes = "api/v{0}/registrationMaterials/permitTypes";
    }

    /// <summary>
    /// Defines the endpoints for the material service.
    /// </summary>
    public static class Material
    {
        public const string GetAllMaterials = "api/v{0}/materials";
    }
}