using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Registration;

[TestClass]
public class RegistrationServiceTests
{
    private Fixture _fixture = null!;
    private Mock<IRegistrationServiceClient> _mockRegistrationServiceClient = null!;

    private RegistrationService _service = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockRegistrationServiceClient = new Mock<IRegistrationServiceClient>();

        _service = new RegistrationService(_mockRegistrationServiceClient.Object);
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationSiteAddressDto>();

        _mockRegistrationServiceClient
            .Setup(client => client.UpdateSiteAddressAsync(registrationId, requestDto))
            .ReturnsAsync(true);
        
        // Act
        var result = await _service.UpdateSiteAddressAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatusAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationTaskStatusDto>();

        _mockRegistrationServiceClient
            .Setup(client => client.UpdateRegistrationTaskStatusAsync(registrationId, requestDto))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateRegistrationTaskStatusAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task UpdateApplicantRegistrationTaskStatusAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationTaskStatusDto>();

        _mockRegistrationServiceClient
            .Setup(client => client.UpdateApplicantRegistrationTaskStatusAsync(registrationId, requestDto))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateApplicantRegistrationTaskStatusAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetRegistrationByOrganisationAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var organisationId = Guid.Empty;
        var registration = new RegistrationDto
        {
            ApplicationTypeId = 1,
            Id = registrationId
        };

        _mockRegistrationServiceClient
            .Setup(client => client.GetRegistrationByOrganisationAsync(1, organisationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _service.GetRegistrationByOrganisationAsync(1, organisationId);

        // Assert
        result.Should().BeEquivalentTo(new RegistrationDto
        {
            ApplicationTypeId = 1,
            Id = registrationId
        });
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationDto>();

        _mockRegistrationServiceClient
            .Setup(client => client.UpdateAsync(registrationId, requestDto))
            .ReturnsAsync(true);

        // Act
        var result = await _service.UpdateAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }
    
    [TestMethod]
    public async Task CreateRegistrationAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var requestDto = _fixture.Create<CreateRegistrationDto>();
        var expectedResponse = _fixture.Create<CreateRegistrationResponseDto>();
        _mockRegistrationServiceClient
            .Setup(client => client.CreateRegistrationAsync(requestDto))
            .ReturnsAsync(expectedResponse);
        // Act
        var result = await _service.CreateRegistrationAsync(requestDto);
        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _mockRegistrationServiceClient.Verify(client => client.CreateRegistrationAsync(requestDto), Times.Once);
    }
    
    [TestMethod]
    public async Task GetRegistrationOverviewAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var expectedOverview = _fixture.Create<RegistrationOverviewDto>();
        _mockRegistrationServiceClient
            .Setup(client => client.GetRegistrationOverviewAsync(registrationId))
            .ReturnsAsync(expectedOverview);
        // Act
        var result = await _service.GetRegistrationOverviewAsync(registrationId);
        // Assert
        result.Should().BeEquivalentTo(expectedOverview);
        _mockRegistrationServiceClient.Verify(client => client.GetRegistrationOverviewAsync(registrationId), Times.Once);
    }
    
    [TestMethod]
    public async Task GetRegistrationsOverviewByOrgIdAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var expectedOverviews = _fixture.Create<IEnumerable<RegistrationsOverviewDto>>();
        _mockRegistrationServiceClient
            .Setup(client => client.GetRegistrationsOverviewByOrgIdAsync(organisationId))
            .ReturnsAsync(expectedOverviews);
        // Act
        var result = await _service.GetRegistrationsOverviewByOrgIdAsync(organisationId);
        // Assert
        result.Should().BeEquivalentTo(expectedOverviews);
        _mockRegistrationServiceClient.Verify(client => client.GetRegistrationsOverviewByOrgIdAsync(organisationId), Times.Once);
    }
}