using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Registrations;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IRegistrationMaterialService> _registrationMaterialService;
    private Mock<ILogger<RegistrationMaterialController>> _loggerMock;
    private RegistrationMaterialController _controller;

    [TestInitialize]
    public void SetUp()
    {
        _registrationMaterialService = new Mock<IRegistrationMaterialService>();
        _loggerMock = new Mock<ILogger<RegistrationMaterialController>>();
        _controller = new RegistrationMaterialController(_registrationMaterialService.Object, _loggerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowArgumentNullException_WhenRegistrationServiceIsNull()
    {
        // Arrange
        Mock<IRegistrationMaterialService> registrationMaterialServiceMock = null;

        // Act
        _ = new RegistrationMaterialController(registrationMaterialServiceMock?.Object, _loggerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        Mock<ILogger<RegistrationMaterialController>> loggerMock = null;

        // Act
        _ = new RegistrationMaterialController(_registrationMaterialService.Object, loggerMock?.Object);
    }
    [TestMethod]
    public async Task CreateExemptionReferences_ShouldReturnBadRequest_WhenDtoIsNull()
    {
        // Act
        var result = await _controller.CreateExemptionReferences(null);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(LogMessages.InvalidRequest)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }
    
    [TestMethod]
    public async Task CreateRegistrationMaterialAndExemptionReferences_ShouldCallServiceAndLogMessage_WhenDtoIsValid()
    {
        // Arrange
        var dto = new CreateExemptionReferencesDto
        {
            MaterialExemptionReferences = new List<MaterialExemptionReferenceDto>(),          
        };

        _registrationMaterialService
            .Setup(s => s.CreateExemptionReferences(dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateExemptionReferences(dto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
        _registrationMaterialService.Verify(s => s.CreateExemptionReferences(dto), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(LogMessages.CreateExemptionReferences)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }
    
    [TestMethod]
    public async Task CreateRegistrationMaterialAndExemptionReferences_ShouldReturnInternalServerError_WhenServiceThrowsException()
    {
        // Arrange
        var dto = new CreateExemptionReferencesDto
        {
            MaterialExemptionReferences = new List<MaterialExemptionReferenceDto>(),          
        };

        var exception = new Exception("Service error");
        _registrationMaterialService
            .Setup(s => s.CreateExemptionReferences(dto))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.CreateExemptionReferences(dto);

        // Assert
        var objectResult = result as ObjectResult;
        Assert.IsNotNull(objectResult);
        Assert.AreEqual(500, objectResult.StatusCode);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(exception.Message)),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }      
}
