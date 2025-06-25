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
        public async Task Get_ShouldReturnDto_WhenApiReturnsDto()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var apiVersion = 1;
            var expectedDto = new CarrierBrokerDealerPermitsDto
            {
                CarrierBrokerDealerPermitId = Guid.NewGuid(),
                RegistrationId = registrationId,
                WasteLicenseOrPermitNumber = "WASTE123",
                PpcNumber = "PPC456",
                WasteExemptionReference = new List<string> { "EX1", "EX2" }
            };

            var config = new PrnBackendServiceApiConfig
            {
                ApiVersion = apiVersion,
                ExportEndpoints = new PrnServiceApiConfigExportEndpoints
                {
                    OtherPermitsGet = "api/v{0}/registrations/{1}/other-permits"
                }
            };

            var mockApiClient = new Mock<IExporterServiceClient>();
            var options = Options.Create(config);

            var expectedUrl = string.Format(config.ExportEndpoints.OtherPermitsGet, apiVersion, registrationId);

            mockApiClient
                .Setup(x => x.SendGetRequest<CarrierBrokerDealerPermitsDto>(expectedUrl))
                .ReturnsAsync(expectedDto);

            var service = new OtherPermitsService(mockApiClient.Object, options);

            // Act
            var result = await service.Get(registrationId);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            mockApiClient.Verify(x => x.SendGetRequest<CarrierBrokerDealerPermitsDto>(expectedUrl), Times.Once);
        }

		[TestMethod]
		public async Task CreateOtherPermits_ShouldReturnNewId()
		{
			// Arrange
			var baseUri = _options.Value.ExportEndpoints.OtherPermitsPost;
			var registrationId = default(Guid);
			var requestDto = new CarrierBrokerDealerPermitsDto
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

			var requestDto = new CarrierBrokerDealerPermitsDto
			{
                CarrierBrokerDealerPermitId = Guid.NewGuid(),
				RegistrationId = registrationId,
				PpcNumber = "ppcNumber",
				WasteExemptionReference = new List<string> { "ref1" },
				WasteLicenseOrPermitNumber = "permitNumber"
			};

			_mockServiceClient
				.Setup(client => client.SendPutRequest<CarrierBrokerDealerPermitsDto>(It.IsAny<string>(), requestDto))
				.Returns(Task.CompletedTask);

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
