namespace Epr.Reproccessor.Exporter.Facade.Api.Models
{
    public class SaveAndContinueModel
    {
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public string? Parameters { get; set; }
        public int RegistrationId { get; set; }
        public string? Area { get; set; }
    }
}
