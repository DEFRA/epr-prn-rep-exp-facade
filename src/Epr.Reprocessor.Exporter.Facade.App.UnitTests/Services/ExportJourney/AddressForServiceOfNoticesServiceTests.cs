using System;
using System.Threading.Tasks;
using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Implementations;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.ExporterJourney
{
    [TestClass]
    public class AddressForServiceOfNoticesServiceTests
    {
        private Mock<IExporterServiceClient> _mockApiClient = null!;
        private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions = null!;
        private AddressForServiceOfNoticesService _service = null!;

        private const int ApiVersion = 1;
        private const string GetUrl = "/api/v{0}/registrations/{1}/carrier-broker-dealer-permits";
        private const string PutUrl = "/api/v{0}/registrations/{1}/carrier-broker-dealer-permits";

        [TestInitialize]
        public void Setup()
        {
            _mockApiClient = new Mock<IExporterServiceClient>();
            _mockOptions = new Mock<IOptions<PrnBackendServiceApiConfig>>();

            var config = new PrnBackendServiceApiConfig
            {
                ApiVersion = ApiVersion,
                ExportEndpoints = new PrnServiceApiConfigExportEndpoints
                {
                    WasteCarrierBrokerDealerRefGet = GetUrl,
                    WasteCarrierBrokerDealerRefPut = PutUrl
                }
            };

            _mockOptions.Setup(x => x.Value).Returns(config);

            _service = new AddressForServiceOfNoticesService(_mockApiClient.Object, _mockOptions.Object);
        }

        [TestMethod]
        public async Task Get_ReturnsDto_WhenApiClientReturnsDto()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var expectedDto = new AddressForServiceOfNoticesDto
            {
                RegistrationId = registrationId,
                LegalDocumentAddress = new AddressDto
                {
                    AddressLine1 = "Test",
                    TownCity = "City",
                    PostCode = "AB12 3CD"
                }
            };
            var expectedUri = string.Format(GetUrl, ApiVersion, registrationId);

            _mockApiClient
                .Setup(x => x.SendGetRequest<AddressForServiceOfNoticesDto>(expectedUri))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _service.Get(registrationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto, result);
        }

        [TestMethod]
        public async Task Update_CallsApiClientWithCorrectParameters_AndReturnsTrue()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var dto = new AddressForServiceOfNoticesDto
            {
                RegistrationId = registrationId,
                LegalDocumentAddress = new AddressDto
                {
                    AddressLine1 = "Test",
                    TownCity = "City",
                    PostCode = "AB12 3CD"
                }
            };
            var expectedUri = string.Format(PutUrl, ApiVersion, registrationId);

            _mockApiClient
                .Setup(x => x.SendPutRequest(expectedUri, dto))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _service.Update(registrationId, dto);

            // Assert
            Assert.IsTrue(result);
            _mockApiClient.Verify(x => x.SendPutRequest(expectedUri, dto), Times.Once);
        }
    }
}
