using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using FluentAssertions;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Accreditation;

[TestClass]
public class OverseasAccreditationSiteServiceTests
{
    private Fixture _fixture = null!;
    private Mock<IOverseasAccreditationSiteServiceClient> _mockClient = null!;
    private OverseasAccreditationSiteService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockClient = new Mock<IOverseasAccreditationSiteServiceClient>();
        _service = new OverseasAccreditationSiteService(_mockClient.Object);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ShouldReturnSiteDtos()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var expected = _fixture.Create<List<OverseasAccreditationSiteDto>>();
        _mockClient.Setup(c => c.GetAllByAccreditationId(accreditationId)).ReturnsAsync(expected);

        // Act
        var result = await _service.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().BeEquivalentTo(expected);
        _mockClient.Verify(c => c.GetAllByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task PostByAccreditationId_ShouldCallClientWithCorrectParameters()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = _fixture.Create<OverseasAccreditationSiteDto>();

        _mockClient.Setup(c => c.PostByAccreditationId(accreditationId, request)).Returns(Task.CompletedTask);

        // Act
        await _service.PostByAccreditationId(accreditationId, request);

        // Assert
        _mockClient.Verify(c => c.PostByAccreditationId(accreditationId, request), Times.Once);
    }
}
