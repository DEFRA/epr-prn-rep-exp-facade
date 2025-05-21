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
    public async Task UpdateSiteAddressAndContactDetails_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = 1;
        var requestDto = _fixture.Create<UpdateSiteAddressDto>();

        _mockRegistrationServiceClient
            .Setup(client => client.UpdateSiteAddress(registrationId, requestDto))
            .ReturnsAsync(true);
        
        // Act
        var result = await _service.UpdateSiteAddress(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }
}
