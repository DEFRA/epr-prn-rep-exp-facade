namespace Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class AccreditationFileUploadDto
{
    public Guid? ExternalId { get; set; }
    public int? OverseasSiteId { get; set; }
    public string Filename { get; set; }
    public Guid? FileId { get; set; }
    public DateTime UploadedOn { get; set; }
    public string UploadedBy { get; set; }
    public int FileUploadTypeId { get; set; }
    public int FileUploadStatusId { get; set; }
}
