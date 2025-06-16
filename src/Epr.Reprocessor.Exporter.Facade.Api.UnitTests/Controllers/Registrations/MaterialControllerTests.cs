using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Registrations;

[TestClass]
public class MaterialControllerTests
{
    private Mock<IMaterialService> _mockMaterialService = null!;
    private Mock<ILogger<MaterialController>> _loggerMock = null!;
    private MaterialController _controller = null!;

    [TestInitialize]
    public void SetUp()
    {
        _mockMaterialService = new Mock<IMaterialService>();
        _loggerMock = new Mock<ILogger<MaterialController>>();
        _controller = new MaterialController(_mockMaterialService.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetAllMaterials_Success()
    {
        // Arrange
        var materials = new List<AvailableMaterialDto>
        {
            new() { Name = "Wood", Code = "W1" }
        };

        // Expectations
        _mockMaterialService.Setup(s => s.GetAllMaterialsAsync()).ReturnsAsync(materials);

        // Act
        var result = await _controller.GetAllMaterials();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        (result as OkObjectResult).Should().BeEquivalentTo(new OkObjectResult(materials));
    }
}