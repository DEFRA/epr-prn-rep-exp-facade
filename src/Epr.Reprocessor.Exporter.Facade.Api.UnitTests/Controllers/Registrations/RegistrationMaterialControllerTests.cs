using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Registrations;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IRegistrationMaterialService> _registrationMaterialService = null!;
    private Mock<ILogger<RegistrationMaterialController>> _loggerMock = null!;
    private RegistrationMaterialController _controller = null!;
    private readonly Fixture _fixture = new();

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
    public async Task
        CreateRegistrationMaterialAndExemptionReferences_ShouldReturnInternalServerError_WhenServiceThrowsException()
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
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(LogMessages.UnExpectedError)),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }

    [TestMethod]
    public async Task CreateRegistrationMaterial_EnsureCreatedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var request = new CreateRegistrationMaterialRequestDto
        {
            Material = "Steel",
            RegistrationId = registrationId
        };

        var response = new CreateRegistrationMaterialResponseDto
        {
            Id = Guid.NewGuid()
        };

        var expectedResult = new CreatedResult(string.Empty, response);

        // Expectations
        _registrationMaterialService.Setup(o => o.CreateRegistrationMaterial(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.CreateRegistrationMaterial(request);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task CreateRegistrationMaterial_NullRequest_ReturnBadRequest()
    {
        // Arrange
        var expectedResult = new BadRequestObjectResult(LogMessages.InvalidRequest);

        // Act
        var result = await _controller.CreateRegistrationMaterial(null);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task CreateRegistrationMaterial_ServiceThrowsException_ReturnInternalError()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var request = new CreateRegistrationMaterialRequestDto
        {
            Material = "Steel",
            RegistrationId = registrationId
        };

        var expectedResult = new ObjectResult(LogMessages.UnExpectedError)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        // Expectations
        _registrationMaterialService
            .Setup(o => o.CreateRegistrationMaterial(request)).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.CreateRegistrationMaterial(request);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermits_ShouldReturnNoContent_WhenUpdateSucceeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = _fixture.Create<UpdateRegistrationMaterialPermitsDto>();
        _registrationMaterialService.Setup(s => s.UpdateRegistrationMaterialPermitsAsync(id, dto))
                    .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateRegistrationMaterialPermits(id, dto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _registrationMaterialService.Verify(s => s.UpdateRegistrationMaterialPermitsAsync(id, dto), Times.Once);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermitCapacity_ShouldReturnNoContent_WhenUpdateSucceeds()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = _fixture.Create<UpdateRegistrationMaterialPermitCapacityDto>();
        _registrationMaterialService.Setup(s => s.UpdateRegistrationMaterialPermitCapacityAsync(id, dto))
                    .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateRegistrationMaterialPermitCapacity(id, dto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _registrationMaterialService.Verify(s => s.UpdateRegistrationMaterialPermitCapacityAsync(id, dto), Times.Once);
    }

    [TestMethod]
    public async Task GetMaterialsPermitTypes_ShouldReturnOkWithData_WhenServiceReturnsList()
    {
        // Arrange
        var expectedList = _fixture.Create<List<MaterialsPermitTypeDto>>();

        _registrationMaterialService.Setup(s => s.GetMaterialsPermitTypesAsync())
                    .ReturnsAsync(expectedList);

        // Act
        var result = await _controller.GetMaterialsPermitTypes();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(expectedList);

        _registrationMaterialService.Verify(s => s.GetMaterialsPermitTypesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllMaterialsForRegistration_EnsureOkResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registrationMaterials = new List<ApplicationRegistrationMaterialDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                RegistrationId = registrationId,
                PPCPermitNumber = "number"
            }
        };

        var expectedResult = new OkObjectResult(registrationMaterials);

        // Expectations
        _registrationMaterialService
            .Setup(s => s.GetAllRegistrationsMaterials(registrationId))
            .ReturnsAsync(registrationMaterials);

        // Act
        var result = await _controller.GetAllRegistrationMaterials(registrationId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetAllMaterialsForRegistration_ServiceException_ReturnInternalServerError()
    {
        // Arrange
        var registrationId = Guid.NewGuid();

        var expectedResult = new ObjectResult(LogMessages.UnExpectedError)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        // Expectations
        _registrationMaterialService
            .Setup(s => s.GetAllRegistrationsMaterials(registrationId))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetAllRegistrationMaterials(registrationId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task DeleteRegistrationMaterial_ServiceException_ReturnInternalServerError()
    {
        // Arrange
        var registrationId = Guid.NewGuid();

        var expectedResult = new ObjectResult(LogMessages.UnExpectedError)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        // Expectations
        _registrationMaterialService
            .Setup(s => s.Delete(registrationId))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _controller.DeleteRegistrationMaterial(registrationId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task DeleteRegistrationMaterial_TrueResponse_ReturnOkResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();

        var expectedResult = new OkResult();

        // Expectations
        _registrationMaterialService
            .Setup(s => s.Delete(registrationId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteRegistrationMaterial(registrationId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task DeleteRegistrationMaterial_FalseResponse_ReturnBadRequestResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();

        var expectedResult = new BadRequestResult();

        // Expectations
        _registrationMaterialService
            .Setup(s => s.Delete(registrationId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteRegistrationMaterial(registrationId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

	[TestMethod]
	public async Task UpdateIsMaterialRegisteredAsync_ShouldReturnNoContent_WhenUpdateSucceeds()
	{
		// Arrange
		var dto = _fixture.Create<List<UpdateIsMaterialRegisteredDto>>();
		_registrationMaterialService.Setup(s => s.UpdateIsMaterialRegisteredAsync(dto))
					.ReturnsAsync(true);

		// Act
		var result = await _controller.UpdateIsMaterialRegisteredAsync(dto);

		// Assert
		result.Should().BeOfType<NoContentResult>();
		_registrationMaterialService.Verify(s => s.UpdateIsMaterialRegisteredAsync(dto), Times.Once);
	}
}