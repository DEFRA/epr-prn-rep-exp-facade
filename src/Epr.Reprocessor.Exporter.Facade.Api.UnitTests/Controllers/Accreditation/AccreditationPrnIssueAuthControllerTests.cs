using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Accreditation;

[TestClass]
public class AccreditationPrnIssueAuthControllerTests
{
    private Mock<IAccreditationPrnIssueAuthService> _serviceMock = null!;
    private AccreditationPrnIssueAuthController _controller = null!;

    [TestInitialize]
    public void SetUp()
    {
        _serviceMock = new Mock<IAccreditationPrnIssueAuthService>();
        _controller = new AccreditationPrnIssueAuthController(_serviceMock.Object);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ShouldReturnOk_WhenEntitiesExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var expected = new List<AccreditationPrnIssueAuthDto>
        {
            new AccreditationPrnIssueAuthDto { ExternalId = Guid.NewGuid() }
        };
        _serviceMock.Setup(s => s.GetByAccreditationId(accreditationId))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expected);
        _serviceMock.Verify(s => s.GetByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ShouldReturnNotFound_WhenNullReturned()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetByAccreditationId(accreditationId))
            .ReturnsAsync((List<AccreditationPrnIssueAuthDto>?)null);

        // Act
        var result = await _controller.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task Post_ShouldReturnNoContent()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new List<AccreditationPrnIssueAuthRequestDto>
        {
            new AccreditationPrnIssueAuthRequestDto { PersonExternalId = Guid.NewGuid() },
            new AccreditationPrnIssueAuthRequestDto { PersonExternalId = Guid.NewGuid() }
        };

        // Act
        var result = await _controller.Post(accreditationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.ReplaceAllByAccreditationId(accreditationId, request), Times.Once);
    }
}
