using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using FluentAssertions;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Accreditation;

[TestClass]
public class AccreditationServiceTests
{
    private Fixture _fixture = null!;
    private Mock<IAccreditationServiceClient> _mockClient = null!;
    private AccreditationService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockClient = new Mock<IAccreditationServiceClient>();
        _service = new AccreditationService(_mockClient.Object);
    }

    [TestMethod]
    public async Task GetOrCreateAccreditation_ShouldReturn_AccreditationId_FromClient()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;

        _mockClient.Setup(c => c.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId))
            .ReturnsAsync(accreditationId);

        // Act
        var result = await _service.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId);

        // Assert
        result.Should().Be(accreditationId);
        _mockClient.Verify(c => c.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId), Times.Once);
    }

    [TestMethod]
    public async Task GetAccreditationById_ShouldReturnDtoFromClient()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var expected = _fixture.Create<AccreditationDto>();
        _mockClient.Setup(c => c.GetAccreditationById(accreditationId))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetAccreditationById(accreditationId);

        // Assert
        result.Should().Be(expected);
        _mockClient.Verify(c => c.GetAccreditationById(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task UpsertAccreditation_ShouldReturnDtoFromClient()
    {
        // Arrange
        var request = _fixture.Create<AccreditationRequestDto>();
        var expected = _fixture.Create<AccreditationDto>();
        _mockClient.Setup(c => c.UpsertAccreditation(request))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.UpsertAccreditation(request);

        // Assert
        result.Should().Be(expected);
        _mockClient.Verify(c => c.UpsertAccreditation(request), Times.Once);
    }
}