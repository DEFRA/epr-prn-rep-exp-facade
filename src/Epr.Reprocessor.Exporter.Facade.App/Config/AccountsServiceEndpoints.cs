using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Config;

[ExcludeFromCodeCoverage]
public class AccountsServiceEndpoints
{
    public string PendingApplications { get; set; } = null!;
    public string GetOrganisationsApplications { get; set; } = null!;
    public string ManageEnrolment { get; set; } = null!;
    public string TransferOrganisationNation { get; set; } = null!;
    public string UserOrganisations { get; set; } = null!;
    public string CreateRegulator { get; set; } = null!;
    public string GetRegulator { get; set; } = null!;
    public string RegulatorInvitation { get; set; } = null!;
    public string RegulatorEnrollment { get; set; } = null!;
    public string RegulatorInvitedUser { get; set; } = null!;
    public string GetRegulatorUsers { get; set; } = null!;
    public string GetOrganisationsBySearchTerm { get; set; } = null!;
    public string GetUsersByOrganisationExternalId { get; set; } = null!;
    public string GetOrganisationDetails { get; set; } = null!;
    public string RegulatorRemoveApprovedUser { get; set; } = null!;
    public string AddRemoveApprovedUser { get; set; } = null!;
}