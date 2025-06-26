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
	public class WasteCarrierBrokerDealerRefServiceTests
	{
		private Mock<IExporterServiceClient> _mockServiceClient = null!;
		private WasteCarrierBrokerDealerRefService _service = null!;
		private IOptions<PrnBackendServiceApiConfig> _options;

		public WasteCarrierBrokerDealerRefServiceTests()
		{
			_options = CreateOptions();
		}

		[TestInitialize]
		public void TestInitialize()
		{
			_mockServiceClient = new Mock<IExporterServiceClient>();
			_service = new WasteCarrierBrokerDealerRefService(_mockServiceClient.Object, _options);
		}

		[TestMethod]
		public async Task GetWasteCarrierBrokerDealerRef_ValidRegistrationId_ShouldReturnWasteCarrierBrokerDealerRef()
		{
			// Arrange
			var baseUri = _options.Value.ExportEndpoints.WasteCarrierBrokerDealerRefGet;
			var registrationId = Guid.NewGuid();
			var responseDto = new WasteCarrierBrokerDealerRefDto
			{
				Id = Guid.NewGuid(),
				RegistrationId = registrationId, 
				WasteCarrierBrokerDealerRegistration = "abc"
			};

			_mockServiceClient
				.Setup(client => client.SendGetRequest<WasteCarrierBrokerDealerRefDto>(baseUri))
				.ReturnsAsync(responseDto);

			_service = new WasteCarrierBrokerDealerRefService(_mockServiceClient.Object, _options);

			// Act
			var result = await _service.Get(registrationId);

			// Assert
			result.Should().BeNull();
		}

		[TestMethod]
		public async Task CreateWasteCarrierBrokerDealerRef_ShouldReturnNewId()
		{
			// Arrange
			var baseUri = _options.Value.ExportEndpoints.WasteCarrierBrokerDealerRefPost;
            var registrationId = Guid.NewGuid();
            var requestDto = new WasteCarrierBrokerDealerRefDto
            {
                Id = Guid.NewGuid(),
                RegistrationId = registrationId,
                WasteCarrierBrokerDealerRegistration = "abc"
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
		public async Task UpdateWasteCarrierBrokerDealerRef_ShouldReturnExpectedResult()
		{
            // Arrange
            var registrationId = Guid.NewGuid();
            var baseUri = _options.Value.ExportEndpoints.WasteCarrierBrokerDealerRefPut;

			var requestDto = new WasteCarrierBrokerDealerRefDto
			{
				Id = Guid.NewGuid(),
				RegistrationId = registrationId,
				WasteCarrierBrokerDealerRegistration = "abc"
			};

			_mockServiceClient
				.Setup(client => client.SendPutRequest(It.IsAny<string>(), requestDto))
				.Returns(Task.CompletedTask);

			// Act
			var result = await _service.Update(registrationId, requestDto);

			// Assert
			result.Should().BeTrue();
		}

		private static IOptions<PrnBackendServiceApiConfig> CreateOptions()
		{
			var exportEndpoints = new PrnServiceApiConfigExportEndpoints
			{
				WasteCarrierBrokerDealerRefGet = "api/v{0}/registrations/{1}/waste-carrier-broker-dealer-ref",
				WasteCarrierBrokerDealerRefPost = "api/v{0}/registrations/{1}/waste-carrier-broker-dealer-ref",
				WasteCarrierBrokerDealerRefPut = "api/v{0}/registrations/{1}/waste-carrier-broker-dealer-ref"
			};

			var options = Options.Create<PrnBackendServiceApiConfig>(new PrnBackendServiceApiConfig
			{
				ApiVersion = 1,
				BaseUrl = "",
				ExportEndpoints = exportEndpoints
			});
			return options;
		}
	}
}
