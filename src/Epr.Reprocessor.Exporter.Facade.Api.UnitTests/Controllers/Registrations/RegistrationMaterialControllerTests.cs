using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Registrations;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IRegistrationService> _mockService;
    private Mock<ILogger<RegistrationMaterialController>> _mockLogger;
    private RegistrationMaterialController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IRegistrationService>();
        _mockLogger = new Mock<ILogger<RegistrationMaterialController>>();
        _controller = new RegistrationMaterialController(_mockService.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterial_ShouldReturnNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateRegistrationMaterialPermitsDto
        {
            PermitTypeId = 2,
            PermitNumber = "AB1234567890",
        };

        _mockService.Setup(s => s.UpdateRegistrationMaterialPermitsAsync(id, request))
                    .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateRegistrationMaterial(id, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _mockService.Verify(s => s.UpdateRegistrationMaterialPermitsAsync(id, request), Times.Once);
    }

    [TestMethod]
    public async Task GetMaterialsPermitTypes_ShouldReturnOkWithData()
    {
        // Arrange
        var permitTypes = new List<MaterialsPermitTypeDto>
            {
                new() { Id = 1, Name = "Type A" },
                new() { Id = 2, Name = "Type B" }
            };

        _mockService.Setup(s => s.GetMaterialsPermitTypesAsync())
            .ReturnsAsync(permitTypes);

        // Act
        var result = await _controller.GetMaterialsPermitTypes();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(permitTypes);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterial_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateRegistrationMaterialPermitsDto
        {
            PermitTypeId = 2,
            PermitNumber = "AB1234567890",
        };

        _mockService.Setup(s => s.UpdateRegistrationMaterialPermitsAsync(id, request))
                    .ThrowsAsync(new Exception("Service failed"));

        // Act & Assert
        Func<Task> act = async () => await _controller.UpdateRegistrationMaterial(id, request);
        await act.Should().ThrowAsync<Exception>().WithMessage("Service failed");
    }

    [TestMethod]
    public async Task GetMaterialsPermitTypes_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        _mockService.Setup(s => s.GetMaterialsPermitTypesAsync())
                    .ThrowsAsync(new Exception("DB error"));

        // Act & Assert
        Func<Task> act = async () => await _controller.GetMaterialsPermitTypes();
        await act.Should().ThrowAsync<Exception>().WithMessage("DB error");
    }
}
