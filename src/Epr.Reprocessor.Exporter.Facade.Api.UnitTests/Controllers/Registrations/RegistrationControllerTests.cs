using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Registrations;


[TestClass]
public class RegistrationControllerTests
{
    private Mock<IRegistrationService> _registrationServiceMock;
    private Mock<ILogger<CountryController>> _loggerMock;
    private CountryController _controller;

    [TestInitialize]
    public void SetUp()
    {
        _registrationServiceMock = new Mock<IRegistrationService>();
        _loggerMock = new Mock<ILogger<CountryController>>();
        _controller = new RegistrationController(_registrationServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowArgumentNullException_WhenRegistrationServiceIsNull()
    {
        // Arrange
        Mock<IRegistrationService> registrationServiceMock = null;

        // Act
        _ = new RegistrationController(registrationServiceMock?.Object, _loggerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        Mock<ILogger<CountryController>> loggerMock =  null;

        // Act
        _ = new RegistrationController(_registrationServiceMock.Object, loggerMock?.Object);
    }

    [TestMethod]
    public async Task UpdateSiteAddress_ShouldReturnNoContentResult()
    {
        // Arrange
        var registrationId = 1;
        var request = new UpdateRegistrationSiteAddressDto();

        // Act
        var result = await _controller.UpdateSiteAddress(registrationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _registrationServiceMock.Verify(s => s.UpdateSiteAddressAsync(registrationId, request), Times.Once);
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatus_ShouldReturnNoContentResult()
    {
        // Arrange
        var registrationId = 1;
        var request = new UpdateRegistrationTaskStatusDto();

        // Act
        var result = await _controller.UpdateRegistrationTaskStatus(registrationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _registrationServiceMock.Verify(s => s.UpdateRegistrationTaskStatusAsync(registrationId, request), Times.Once);
    }
}
