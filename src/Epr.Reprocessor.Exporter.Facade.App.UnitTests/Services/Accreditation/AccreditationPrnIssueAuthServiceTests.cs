using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using FluentAssertions;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Accreditation;

[TestClass]
public class AccreditationPrnIssueAuthServiceTests
{
    private Fixture _fixture = null!;
    private Mock<IAccreditationPrnIssueAuthServiceClient> _mockClient = null!;
    private AccreditationPrnIssueAuthService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockClient = new Mock<IAccreditationPrnIssueAuthServiceClient>();
        _service = new AccreditationPrnIssueAuthService(_mockClient.Object);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ShouldReturnListFromClient()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var expected = _fixture.Create<List<AccreditationPrnIssueAuthDto>>();
        _mockClient.Setup(c => c.GetByAccreditationId(accreditationId))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().BeEquivalentTo(expected);
        _mockClient.Verify(c => c.GetByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task ReplaceAllByAccreditationId_ShouldCallClientWithCorrectParameters()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var requestDtos = _fixture.Create<List<AccreditationPrnIssueAuthRequestDto>>();

        _mockClient.Setup(c => c.ReplaceAllByAccreditationId(accreditationId, requestDtos))
            .Returns(Task.CompletedTask);

        // Act
        await _service.ReplaceAllByAccreditationId(accreditationId, requestDtos);

        // Assert
        _mockClient.Verify(c => c.ReplaceAllByAccreditationId(accreditationId, requestDtos), Times.Once);
    }
}
