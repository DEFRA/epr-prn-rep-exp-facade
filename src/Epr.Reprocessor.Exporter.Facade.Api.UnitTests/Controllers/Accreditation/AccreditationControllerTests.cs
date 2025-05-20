using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Accreditation;

[TestClass]
public class AccreditationControllerTests
{
    private Mock<IAccreditationService> _serviceMock;
    private AccreditationController _controller;

    [TestInitialize]
    public void SetUp()
    {
        _serviceMock = new Mock<IAccreditationService>();
        _controller = new AccreditationController(_serviceMock.Object);
    }

    [TestMethod]
    public async Task Get_ShouldReturnOk_WhenAccreditationExists()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var accreditation = new AccreditationDto { ExternalId = accreditationId };
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Get(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.GetAccreditationById(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task Get_ShouldReturnNotFound_WhenAccreditationDoesNotExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync((AccreditationDto?)null);

        // Act
        var result = await _controller.Get(accreditationId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetAccreditationById(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task Post_ShouldReturnOk_WithCreatedAccreditation()
    {
        // Arrange
        var request = new AccreditationRequestDto { OrganisationId = Guid.NewGuid() };
        var accreditation = new AccreditationDto { OrganisationId = request.OrganisationId };
        _serviceMock.Setup(s => s.UpsertAccreditation(request))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Post(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.UpsertAccreditation(request), Times.Once);
    }
}
