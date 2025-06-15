using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Registration;

[TestClass]
public class RegistrationMaterialServiceTests
{

    private Fixture _fixture = null!;
    private Mock<IRegistrationMaterialServiceClient> _mockRegistrationMaterialServiceClient = null!;

    private RegistrationMaterialService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockRegistrationMaterialServiceClient = new Mock<IRegistrationMaterialServiceClient>();

        _service = new RegistrationMaterialService(_mockRegistrationMaterialServiceClient.Object);
    }
    [TestMethod]
    public async Task CreateRegistrationMaterialAndExemptionReferences_CallsClientWithCorrectDto()
    {
        // Arrange
        var dto = _fixture.Create<CreateExemptionReferencesDto>();

        // Act
        await _service.CreateExemptionReferences(dto);

        // Assert
        _mockRegistrationMaterialServiceClient.Verify(
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
        _mockRegistrationMaterialServiceClient.Verify(
            x => x.CreateRegistrationMaterialAsync(
                It.Is<CreateRegistrationMaterialRequestDto>(d => d == dto)),
            Times.Once);
    }
}