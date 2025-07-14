using System;
using System.Threading.Tasks;
using Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.ExporterJourney
{
    [TestClass]
    public class AddressForServiceOfNoticesControllerTests
    {
        private Mock<IAddressForServiceOfNoticesService> _mockService = null!;
        private Mock<ILogger<AddressForServiceOfNoticesController>> _mockLogger = null!;
        private AddressForServiceOfNoticesController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IAddressForServiceOfNoticesService>();
            _mockLogger = new Mock<ILogger<AddressForServiceOfNoticesController>>();
            _controller = new AddressForServiceOfNoticesController(_mockService.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task Get_ReturnsOk_WhenDtoExists()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var dto = new AddressForServiceOfNoticesDto
            {
                RegistrationId = registrationId,
                LegalDocumentAddress = new AddressDto
                {
                    AddressLine1 = "123 Test St",
                    TownCity = "Testville",
                    PostCode = "TST 123"
                }
            };
            _mockService.Setup(s => s.Get(registrationId)).ReturnsAsync(dto);

            // Act
            var result = await _controller.Get(registrationId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(dto, okResult.Value);
        }

        [TestMethod]
        public async Task Get_ReturnsNotFound_WhenDtoIsNull()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            _mockService.Setup(s => s.Get(registrationId)).ReturnsAsync((AddressForServiceOfNoticesDto?)null);

            // Act
            var result = await _controller.Get(registrationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Put_ReturnsAccepted_WhenUpdateSucceeds()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var dto = new AddressForServiceOfNoticesDto
            {
                RegistrationId = registrationId,
                LegalDocumentAddress = new AddressDto
                {
                    AddressLine1 = "456 Test Ave",
                    TownCity = "Sampletown",
                    PostCode = "SMP 456"
                }
            };
            _mockService.Setup(s => s.Update(registrationId, dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Put(registrationId, dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
        }
    }
}
