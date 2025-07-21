using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
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
    public async Task GetOrCreateAccreditation_ShouldReturnOk()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;

        _serviceMock.Setup(s => s.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId))
            .ReturnsAsync(accreditationId);

        // Act
        var result = await _controller.Get(organisationId, materialId, applicationTypeId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditationId);
        _serviceMock.Verify(s => s.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId), Times.Once);
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

    [TestMethod]
    public async Task GetFileUpload_ShouldReturnOk_WithFileUpload()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var expectedDto = new AccreditationFileUploadDto
        {
            ExternalId = externalId,
            Filename = "testfile.txt",
            FileId = Guid.NewGuid(),
            UploadedOn = DateTime.UtcNow,
            UploadedBy = "tester",
            FileUploadTypeId = 1,
            FileUploadStatusId = 2
        };

        _serviceMock.Setup(s => s.GetFileUpload(externalId))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _controller.GetFileUpload(externalId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expectedDto);
        _serviceMock.Verify(s => s.GetFileUpload(externalId), Times.Once);
    }

    [TestMethod]
    public async Task GetFileUpload_ShouldReturn_NotFound()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var expected = new AccreditationFileUploadDto { FileId = Guid.NewGuid(), Filename = "file1.txt" };

        _serviceMock.Setup(s => s.GetFileUpload(externalId))
            .ReturnsAsync((AccreditationFileUploadDto)null);

        // Act
        var result = await _controller.GetFileUpload(externalId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetFileUpload(externalId), Times.Once);
    }

    [TestMethod]
    public async Task GetFileUploads_ShouldReturnOk_WithFileUploads()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadTypeId = 1;
        var fileUploadStatusId = 2;
        var expected = new List<AccreditationFileUploadDto>
        {
            new AccreditationFileUploadDto { FileId = Guid.NewGuid(), Filename = "file1.txt" }
        };

        _serviceMock.Setup(s => s.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expected);
        _serviceMock.Verify(s => s.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId), Times.Once);
    }

    [TestMethod]
    public async Task GetFileUploads_FileUploadsNull_ReturnNotFound()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadTypeId = 1;
        var fileUploadStatusId = 2;

        _serviceMock.Setup(s => s.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId))
            .ReturnsAsync((List<AccreditationFileUploadDto>?)null);

        // Act
        var result = await _controller.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId), Times.Once);
    }

    [TestMethod]
    public async Task UpsertFileUpload_ShouldReturnOk_WithFileUpload()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new AccreditationFileUploadDto { FileId = Guid.NewGuid(), Filename = "file2.txt" };
        var expected = new AccreditationFileUploadDto { FileId = request.FileId, Filename = request.Filename };

        _serviceMock.Setup(s => s.UpsertFileUpload(accreditationId, request))
            .ReturnsAsync(expected);

        // Act
        var result = await _controller.UpsertFileUpload(accreditationId, request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(expected);
        _serviceMock.Verify(s => s.UpsertFileUpload(accreditationId, request), Times.Once);
    }

    [TestMethod]
    public async Task DeleteFileUpload_ShouldReturnOk()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileId = Guid.NewGuid();

        _serviceMock.Setup(s => s.DeleteFileUpload(accreditationId, fileId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteFileUpload(accreditationId, fileId);

        // Assert
        result.Should().BeOfType<OkResult>();
        _serviceMock.Verify(s => s.DeleteFileUpload(accreditationId, fileId), Times.Once);
    }

    [TestMethod]
    public async Task GetFileUpload_ReturnsOk_WhenFileUploadExists()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var fileUploadDto = new AccreditationFileUploadDto { ExternalId = externalId };
        _serviceMock.Setup(s => s.GetFileUpload(externalId))
            .ReturnsAsync(fileUploadDto);

        // Act
        var result = await _controller.GetFileUpload(externalId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(fileUploadDto);
    }

    [TestMethod]
    public async Task GetFileUpload_ReturnsNotFound_WhenFileUploadDoesNotExist()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetFileUpload(externalId))
            .ReturnsAsync((AccreditationFileUploadDto)null);

        // Act
        var result = await _controller.GetFileUpload(externalId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public async Task GetAccreditationOverviewByOrgId_NoItems_ReturnsEmptyList()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var expectedOutput = new List<AccreditationOverviewDto>();
        _serviceMock.Setup(x => x.GetAccreditationOverviewByOrgId(orgId))
            .ReturnsAsync(new List<AccreditationOverviewDto>());

        // Act
        var result = await _controller.GetAccreditationOverviewByOrgId(orgId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult!.Value.Should().BeEquivalentTo(expectedOutput);
    }

    [TestMethod]
    public async Task GetAccreditationOverviewByOrgId_3Items_ReturnsExpectedList()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var expectedOutput = new List<AccreditationOverviewDto>
        {
            new AccreditationOverviewDto
            {
                OrganisationId = orgId,
            },
            new AccreditationOverviewDto
            {
                OrganisationId = orgId
            },
            new AccreditationOverviewDto
            {
                OrganisationId = orgId
            }
        };
        _serviceMock.Setup(x => x.GetAccreditationOverviewByOrgId(orgId))
            .ReturnsAsync(expectedOutput);

        // Act
        var result = await _controller.GetAccreditationOverviewByOrgId(orgId);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult!.Value.Should().BeEquivalentTo(expectedOutput);
    }
}
