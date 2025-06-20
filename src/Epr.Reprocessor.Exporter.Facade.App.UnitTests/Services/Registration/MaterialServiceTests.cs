using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Registration;

[TestClass]
public class MaterialServiceTests
{
    private Mock<IMaterialServiceClient> _mockMaterialServiceClient = null!;
    private MaterialService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMaterialServiceClient = new Mock<IMaterialServiceClient>();
        _service = new MaterialService(_mockMaterialServiceClient.Object);
    }

    [TestMethod]
    public async Task GetMaterials()
    {
        // Arrange
        var materials = new List<AvailableMaterialDto>
        {
            new() { Name = "Wood", Code = "W1" }
        };

        _mockMaterialServiceClient
            .Setup(client => client.GetAllMaterialsAsync())
            .ReturnsAsync(materials);

        // Act
        var result = await _service.GetAllMaterialsAsync();

        // Assert
        result.Should().BeEquivalentTo(materials);
    }
}