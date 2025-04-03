namespace Epr.Reproccessor.Exporter.Facade.Api.Models
{
    public class SaveAndContinueResponse
    {
        public int Id { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public string? Parameters { get; set; }
        public int RegistrationId { get; set; }
        public string? Area { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
