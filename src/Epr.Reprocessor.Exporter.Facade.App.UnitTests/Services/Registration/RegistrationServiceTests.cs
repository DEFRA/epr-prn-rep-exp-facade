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
}
