using Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.ExporterJourney
{
    [TestClass]
    public class WasteCarrierBrokerDealerRefControllerTests
    {
        private Mock<IWasteCarrierBrokerDealerRefService> _serviceMock;
        private Mock<ILogger<WasteCarrierBrokerDealerRefController>> _loggerMock;
        private WasteCarrierBrokerDealerRefController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _serviceMock = new Mock<IWasteCarrierBrokerDealerRefService>();
            _loggerMock = new Mock<ILogger<WasteCarrierBrokerDealerRefController>>();
            _controller = new WasteCarrierBrokerDealerRefController(_serviceMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenServiceIsNull()
        {
            var controller = new WasteCarrierBrokerDealerRefController(null, _loggerMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            var controller = new WasteCarrierBrokerDealerRefController(_serviceMock.Object, null);
        }

        [TestMethod]
        public async Task Get_ShouldReturnOk_WhenDataObjectWithRegistrationIdExists()
        {
            var registrationId = Guid.NewGuid();
            var dto = new WasteCarrierBrokerDealerRefDto { RegistrationId = registrationId };
            _serviceMock.Setup(s => s.Get(registrationId)).ReturnsAsync(dto);

            var result = await _controller.Get(registrationId);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().Be(dto);
            _serviceMock.Verify(s => s.Get(registrationId), Times.Once);
        }

        [TestMethod]
        public async Task Get_ShouldReturnNotFound_WhenDataObjectDoesNotExist()
        {
            var registrationId = Guid.NewGuid();
            _serviceMock.Setup(s => s.Get(registrationId)).ReturnsAsync((WasteCarrierBrokerDealerRefDto?)null);

            var result = await _controller.Get(registrationId);

            result.Should().BeOfType<NotFoundResult>();
            _serviceMock.Verify(s => s.Get(registrationId), Times.Once);
        }

        [TestMethod]
        public async Task Post_ShouldReturnOk_WithCreatedId()
        {
            var registrationId = Guid.NewGuid();
            var request = new WasteCarrierBrokerDealerRefDto { RegistrationId = registrationId };
            var createdId = Guid.NewGuid();
            _serviceMock.Setup(s => s.Create(registrationId, request)).ReturnsAsync(createdId);

            var result = await _controller.Post(request);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().Be(createdId);
            _serviceMock.Verify(s => s.Create(registrationId, request), Times.Once);
        }

        [TestMethod]
        public async Task Put_ShouldReturnAccepted_WhenUpdateSucceeds()
        {
            var registrationId = Guid.NewGuid();
            var request = new WasteCarrierBrokerDealerRefDto();
            _serviceMock.Setup(s => s.Update(registrationId, request)).ReturnsAsync(true);

            var result = await _controller.Put(registrationId, request);

            result.Should().BeOfType<AcceptedResult>();
            _serviceMock.Verify(s => s.Update(registrationId, request), Times.Once);
        }
    }
}