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
}
