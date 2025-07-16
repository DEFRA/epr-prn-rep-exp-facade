using System.Security.Claims;
using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Registrations;

[TestClass]
public class ExporterControllerTests
{
    private Mock<IRegistrationMaterialService> _registrationMaterialServiceMock = null!;
    private Mock<ILogger<ExporterController>> _loggerMock = null!;
    private ExporterController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _registrationMaterialServiceMock = new Mock<IRegistrationMaterialService>();
        _loggerMock = new Mock<ILogger<ExporterController>>();
        _controller = new ExporterController(_registrationMaterialServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task SaveOverseasReprocessor_ReturnsBadRequest_WhenRequestIsNull()
    {
        // Act
        var result = await _controller.SaveOverseasReprocessor(null!);

        // Assert
        using var scope = new AssertionScope();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be(LogMessages.InvalidRequest);
        _loggerMock.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(LogMessages.InvalidRequest)),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [TestMethod]
    public async Task SaveOverseasReprocessor_ShouldReturnNoContent_WhenServiceSucceeds()
    {
        // Arrange
        var request = new OverseasAddressRequest();
        var userId = Guid.NewGuid();
        _registrationMaterialServiceMock
            .Setup(s => s.SaveOverseasReprocessorAsync(request, userId))
            .ReturnsAsync(true);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userId.ToString())
        }));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.SaveOverseasReprocessor(request);

        // Assert
        using var scope = new AssertionScope();
        result.Should().BeOfType<NoContentResult>();
        _registrationMaterialServiceMock.Verify(s => s.SaveOverseasReprocessorAsync(request, userId), Times.Once);
    }

    [TestMethod]
    public async Task SaveOverseasReprocessor_ReturnsInternalServerError_WhenExceptionThrown()
    {
        // Arrange
        var request = new OverseasAddressRequest
        {
            RegistrationMaterialId = Guid.NewGuid(),
            OverseasAddresses = new List<OverseasAddress>()
        };
        _registrationMaterialServiceMock
            .Setup(x => x.SaveOverseasReprocessorAsync(request, Guid.NewGuid()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.SaveOverseasReprocessor(request);

        // Assert
        using var scope = new AssertionScope();
        var objectResult = result as ObjectResult;
        objectResult.Should().NotBeNull();
        objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        objectResult.Value.Should().Be(LogMessages.UnExpectedError);
        _loggerMock.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(LogMessages.UnExpectedError)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [TestMethod]
    public void Constructor_ThrowsArgumentNullException_WhenServiceIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ExporterController(null!, _loggerMock.Object));
    }

    [TestMethod]
    public void Constructor_ThrowsArgumentNullException_WhenLoggerIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ExporterController(_registrationMaterialServiceMock.Object, null!));
    }
}
