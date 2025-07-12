using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Accreditation;

[TestClass]
public class OverseasAccreditationSiteControllerTests
{
    private Mock<IOverseasAccreditationSiteService> _serviceMock = null!;
    private OverseasAccreditationSiteController _controller = null!;

    [TestInitialize]
    public void SetUp()
    {
        _serviceMock = new Mock<IOverseasAccreditationSiteService>();
        _controller = new OverseasAccreditationSiteController(_serviceMock.Object);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ShouldReturnSiteDtos_WhenEntitiesExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var siteDtos = new List<OverseasAccreditationSiteDto>
        {
            new OverseasAccreditationSiteDto {
                ExternalId = accreditationId, OrganisationName = "Hun Manet Recycler Ltd", MeetConditionsOfExportId = 1, SiteCheckStatusId = 2
            }
        };
        _serviceMock.Setup(s => s.GetAllByAccreditationId(accreditationId)).ReturnsAsync(siteDtos);

        // Act
        var result = await _controller.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(siteDtos);
        _serviceMock.Verify(s => s.GetAllByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ShouldReturnNotFound_WhenNullReturnedByService()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetAllByAccreditationId(accreditationId)).ReturnsAsync((List<OverseasAccreditationSiteDto>?)null);

        // Act
        var result = await _controller.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetAllByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task Post_ShouldReturnNoContent()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new OverseasAccreditationSiteDto {
            ExternalId = accreditationId, OrganisationName = "Hun Manet Recycler Ltd", MeetConditionsOfExportId = 2, SiteCheckStatusId = 2
        };

        // Act
        var result = await _controller.Post(accreditationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.PostByAccreditationId(accreditationId, request), Times.Once);
    }
}
