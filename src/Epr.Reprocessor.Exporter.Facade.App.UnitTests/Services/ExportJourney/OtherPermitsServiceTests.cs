using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.ExportJourney
{
	[TestClass]
	public class OtherPermitsServiceTests
	{
		private Mock<IExporterServiceClient> _mockServiceClient = null!;
		private IOtherPermitsService _service = null!;
		private IOptions<PrnBackendServiceApiConfig> _options;

        public OtherPermitsServiceTests()
        {
			_options = CreateOptions(); 
        }

        [TestInitialize]
		public void TestInitialize()
		{
			_mockServiceClient = new Mock<IExporterServiceClient>();
			_service = new OtherPermitsService(_mockServiceClient.Object, _options);
		}

		[TestMethod]
		[Ignore]
		public async Task GetOtherPermits_ValidRegistrationId_ShouldReturnOtherPermits()
		{
			// Arrange
			var baseUri = _options.Value.ExportEndpoints.OtherPermitsGet;
			var registrationId = default(Guid);
			var responseDto = new OtherPermitsDto
			{
				Id = Guid.NewGuid(),
				RegistrationId = registrationId,
				PpcNumber = "ppcNumber",
				WasteExemptionReference = new List<string>() { "ref1" },
				WasteLicenseOrPermitNumber = "permitNumber"
			};

			_mockServiceClient
				.Setup(client => client.SendGetRequest<OtherPermitsDto>(baseUri))
				.ReturnsAsync(responseDto);

			_service = new OtherPermitsService(_mockServiceClient.Object, _options);

			// Act
			var result = await _service.Get(registrationId);

			// Assert
			result.Should().BeNull();
		}

		[TestMethod]
		public async Task CreateOtherPermits_ShouldReturnNewId()
		{
			// Arrange
			var baseUri = _options.Value.ExportEndpoints.OtherPermitsPost;
			var registrationId = default(Guid);
			var requestDto = new OtherPermitsDto
			{
				RegistrationId = registrationId,
				PpcNumber = "ppcNumber",
				WasteExemptionReference = new List<string>() { "ref1" },
				WasteLicenseOrPermitNumber = "permitNumber"
			};

			var newId = Guid.NewGuid();

			_mockServiceClient
				.Setup(client => client.SendPostRequest(It.IsAny<string>(), requestDto))
				.ReturnsAsync(newId);

			// Act
			var result = await _service.Create(registrationId, requestDto);

			// Assert
			result.Should().Be(newId);
		}

		[TestMethod]
		public async Task UpdateOtherPermits_ShouldReturnExpectedResult()
		{
			// Arrange
			var registrationId = default(Guid);
			var baseUri = _options.Value.ExportEndpoints.OtherPermitsPut;

			var requestDto = new OtherPermitsDto
			{
				Id = Guid.NewGuid(),
				RegistrationId = registrationId,
				PpcNumber = "ppcNumber",
				WasteExemptionReference = new List<string> { "ref1" },
				WasteLicenseOrPermitNumber = "permitNumber"
			};

			_mockServiceClient
				.Setup(client => client.SendPutRequest(It.IsAny<string>(), requestDto))
				.ReturnsAsync(requestDto);

			// Act
			var result = await _service.Update(registrationId, requestDto);

			// Assert
			result.Should().BeTrue();
		}

		private IOptions<PrnBackendServiceApiConfig> CreateOptions()
		{
			var exportEndpoints = new PrnServiceApiConfigExportEndpoints
			{
				OtherPermitsGet = "api/v{0}/registrations/{1}/other-permits",
				OtherPermitsPost = "api/v{0}/registrations/{1}/other-permits",
				OtherPermitsPut = "api/v{0}/registrations/{1}/other-permits/{2}"
			};

			var options = Options.Create<PrnBackendServiceApiConfig>(new PrnBackendServiceApiConfig { 
				ApiVersion = 1, 
				BaseUrl = "", 
				ExportEndpoints = exportEndpoints});
			return options;
		}
	}
}
