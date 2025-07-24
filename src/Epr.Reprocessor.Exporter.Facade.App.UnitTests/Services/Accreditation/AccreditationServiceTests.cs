using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Accreditation;
using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;

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

    [TestMethod]
    public async Task GetFileUploads_ShouldReturnListFromClient()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadTypeId = 1;
        var fileUploadStatusId = 2;
        var expected = _fixture.Create<List<AccreditationFileUploadDto>>();

        _mockClient.Setup(c => c.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId);

        // Assert
        result.Should().BeEquivalentTo(expected);
        _mockClient.Verify(c => c.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId), Times.Once);
    }

    [TestMethod]
    public async Task UpsertFileUpload_ShouldReturnDtoFromClient()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = _fixture.Create<AccreditationFileUploadDto>();
        var expected = _fixture.Create<AccreditationFileUploadDto>();

        _mockClient.Setup(c => c.UpsertFileUpload(accreditationId, request))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.UpsertFileUpload(accreditationId, request);

        // Assert
        result.Should().BeEquivalentTo(expected);
        _mockClient.Verify(c => c.UpsertFileUpload(accreditationId, request), Times.Once);
    }

    [TestMethod]
    public async Task DeleteFileUpload_ShouldCallClient()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileId = Guid.NewGuid();

        _mockClient.Setup(c => c.DeleteFileUpload(accreditationId, fileId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteFileUpload(accreditationId, fileId);

        // Assert
        _mockClient.Verify(c => c.DeleteFileUpload(accreditationId, fileId), Times.Once);
    }

    [TestMethod]
    public async Task GetAccreditationOverviewByOrgId_NoItems_ReturnsEmptylist()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var expectedOutput = new List<AccreditationOverviewDto>();
        _mockClient.Setup(x => x.GetAccreditationOverviewByOrgId(orgId))
            .ReturnsAsync(new List<AccreditationOverviewDto>());

        // Act
        var result = await _service.GetAccreditationOverviewByOrgId(orgId);

        // Assert
        result.Should().BeEquivalentTo(expectedOutput);
    }

    [TestMethod]
    public async Task GetAccreditationOverviewByOrgId_3Items_ReturnsListWith3Items()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var expectedOutput = _fixture.CreateMany<AccreditationOverviewDto>(3).ToList();
        _mockClient.Setup(x => x.GetAccreditationOverviewByOrgId(orgId))
            .ReturnsAsync(expectedOutput);

        // Act
        var result = await _service.GetAccreditationOverviewByOrgId(orgId);

        // Assert
        result.Should().BeEquivalentTo(expectedOutput);
    }
}