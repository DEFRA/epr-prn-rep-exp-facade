using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Registration;

[TestClass]
public class RegistrationMaterialServiceTests
{

    private Fixture _fixture = null!;
    private Mock<IRegistrationMaterialServiceClient> _clientMock = null!;

    private RegistrationMaterialService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _clientMock = new Mock<IRegistrationMaterialServiceClient>();

        _service = new RegistrationMaterialService(_clientMock.Object);
    }

    [TestMethod]
    public async Task CreateRegistrationMaterialAndExemptionReferences_CallsClientWithCorrectDto()
    {
        // Arrange
        var dto = _fixture.Create<CreateExemptionReferencesDto>();

        // Act
        await _service.CreateExemptionReferences(dto);

        // Assert
        _clientMock.Verify(
            x => x.CreateExemptionReferencesAsync(
                It.Is<CreateExemptionReferencesDto>(d => d == dto)),
            Times.Once);
    }

    [TestMethod]
    public async Task CreateRegistrationMaterial_CallsClientWithCorrectDto()
    {
        // Arrange
        var dto = _fixture.Create<CreateRegistrationMaterialRequestDto>();

        // Act
        await _service.CreateRegistrationMaterial(dto);

        // Assert
        _clientMock.Verify(
            x => x.CreateRegistrationMaterialAsync(
                It.Is<CreateRegistrationMaterialRequestDto>(d => d == dto)),
            Times.Once);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermitsAsync_ShouldReturnTrue_WhenClientReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateRegistrationMaterialPermitsDto>();
        _clientMock.Setup(x => x.UpdateRegistrationMaterialPermitsAsync(id, request))
                   .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateRegistrationMaterialPermitsAsync(id, request);

        // Assert
        result.Should().BeTrue();
        _clientMock.Verify(x => x.UpdateRegistrationMaterialPermitsAsync(id, request), Times.Once);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermitsAsync_ShouldReturnFalse_WhenClientReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateRegistrationMaterialPermitsDto>();
        _clientMock.Setup(x => x.UpdateRegistrationMaterialPermitsAsync(id, request))
                   .ReturnsAsync(false);

        // Act
        var result = await _service.UpdateRegistrationMaterialPermitsAsync(id, request);

        // Assert
        result.Should().BeFalse();
        _clientMock.Verify(x => x.UpdateRegistrationMaterialPermitsAsync(id, request), Times.Once);
    }


    [TestMethod]
    public async Task UpdateRegistrationMaterialPermitCapacityAsync_ShouldReturnFalse_WhenClientReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateRegistrationMaterialPermitCapacityDto>();
        _clientMock.Setup(x => x.UpdateRegistrationMaterialPermitCapacityAsync(id, request))
                   .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateRegistrationMaterialPermitCapacityAsync(id, request);

        // Assert
        result.Should().BeTrue();
        _clientMock.Verify(x => x.UpdateRegistrationMaterialPermitCapacityAsync(id, request), Times.Once);
    }

    [TestMethod]
    public async Task GetMaterialsPermitTypesAsync_ShouldReturnExpectedList()
    {
        // Arrange
        var expectedList = _fixture.Create<List<MaterialsPermitTypeDto>>();
        _clientMock.Setup(x => x.GetMaterialsPermitTypesAsync())
                   .ReturnsAsync(expectedList);

        // Act
        var result = await _service.GetMaterialsPermitTypesAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedList);
        _clientMock.Verify(x => x.GetMaterialsPermitTypesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllRegistrationMaterials_ShouldReturnExpectedResult()
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
        _clientMock
            .Setup(client => client.GetAllRegistrationMaterialsAsync(registrationId))
            .ReturnsAsync(registrationMaterials);

        // Act
        var result = await _service.GetAllRegistrationsMaterials(registrationId);

        // Assert
        result.Should().BeEquivalentTo(registrationMaterials);
    }

    [TestMethod]
    public async Task Delete_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();

        _clientMock
            .Setup(client => client.DeleteAsync(registrationId))
            .ReturnsAsync(true);

        // Act
        var result = await _service.Delete(registrationId);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task UpdateMaximumWeight_ShouldReturnTrue_WhenClientReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateMaximumWeightDto>();
        _clientMock.Setup(x => x.UpdateMaximumWeightAsync(id, request))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateMaximumWeight(id, request);

        // Assert
        result.Should().BeTrue();
        _clientMock.Verify(x => x.UpdateMaximumWeightAsync(id, request), Times.Once);
    }

    [TestMethod]
    public void UpdateRegistrationTaskStatusAsync_ShouldReturnTrue_WhenClientReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateRegistrationTaskStatusDto>();
        _clientMock.Setup(x => x.UpdateRegistrationTaskStatusAsync(id, request))
            .ReturnsAsync(true);

        // Act
        var task = _service.UpdateRegistrationTaskStatusAsync(id, request);
        
        // Assert
        Assert.IsTrue(task.IsCompleted);
        _clientMock.Verify(x => x.UpdateRegistrationTaskStatusAsync(id, request), Times.Once);
    }
}