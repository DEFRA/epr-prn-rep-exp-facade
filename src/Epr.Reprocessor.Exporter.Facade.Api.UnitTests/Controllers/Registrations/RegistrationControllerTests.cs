using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Services.Registration;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.Registrations;


[TestClass]
public class RegistrationControllerTests
{
    private Mock<IRegistrationService> _registrationServiceMock = null!;
    private Mock<ILogger<RegistrationController>> _loggerMock = null!;
    private RegistrationController _controller = null!;

    [TestInitialize]
    public void SetUp()
    {
        _registrationServiceMock = new Mock<IRegistrationService>();
        _loggerMock = new Mock<ILogger<RegistrationController>>();
        _controller = new RegistrationController(_registrationServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowArgumentNullException_WhenRegistrationServiceIsNull()
    {
        // Arrange
        Mock<IRegistrationService>? registrationServiceMock = null;

        // Act
        _ = new RegistrationController(registrationServiceMock?.Object, _loggerMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        // Arrange
        Mock<ILogger<RegistrationController>>? loggerMock =  null;

        // Act
        _ = new RegistrationController(_registrationServiceMock.Object, loggerMock?.Object);
    }

    [TestMethod]
    public async Task UpdateSiteAddress_ShouldReturnNoContentResult()
    {
        // Arrange
        var registrationId = 1;
        var request = new UpdateRegistrationSiteAddressDto();

        // Act
        var result = await _controller.UpdateSiteAddress(registrationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _registrationServiceMock.Verify(s => s.UpdateSiteAddressAsync(registrationId, request), Times.Once);
    }

    [TestMethod]
    public async Task UpdateRegistrationTaskStatus_ShouldReturnNoContentResult()
    {
        // Arrange
        var registrationId = 1;
        var request = new UpdateRegistrationTaskStatusDto();

        // Act
        var result = await _controller.UpdateRegistrationTaskStatus(registrationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _registrationServiceMock.Verify(s => s.UpdateRegistrationTaskStatusAsync(registrationId, request), Times.Once);
    }

    [TestMethod]
    public async Task CreateRegistration_EmptyValues()
    {
        // Arrange
        var request = new CreateRegistrationDto();

        // Act
        var result = await _controller.CreateRegistration(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [TestMethod]
    public async Task CreateRegistration_PopulatedValuesShouldCallService()
    {
        // Arrange
        var request = new CreateRegistrationDto
        {
            ApplicationTypeId = 1,
            OrganisationId = Guid.NewGuid(),
            ReprocessingSiteAddress = new()
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                TownCity = "Town",
                County = "County",
                Country = "Country",
                GridReference = "T12345",
                PostCode = "postcode"
            }
        };

        var expectedResult = new CreatedResult(string.Empty, 1);

        // Expectations
        _registrationServiceMock
            .Setup(s => s.CreateRegistrationAsync(request))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.CreateRegistration(request);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task UpdateRegistration_PopulatedValuesShouldCallService()
    {
        // Arrange
        var request = new UpdateRegistrationDto
        {
            ReprocessingSiteAddress = new()
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                TownCity = "Town",
                County = "County",
                Country = "Country",
                GridReference = "T12345",
                PostCode = "postcode"
            },
            LegalAddress = new()
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                TownCity = "Town",
                County = "County",
                Country = "Country",
                GridReference = "T12345",
                PostCode = "postcode"
            },
            BusinessAddress = new()
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                TownCity = "Town",
                County = "County",
                Country = "Country",
                GridReference = "T12345",
                PostCode = "postcode"
            }
        };

        // Expectations
        _registrationServiceMock
            .Setup(s => s.UpdateAsync(1, request))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateAsync(1, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [TestMethod]
    public async Task GetRegistrationByOrganisation_RegistrationFound_ShouldReturnOkResult()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registration = new RegistrationDto
        {
            ApplicationTypeId = 1,
            ExternalId = Guid.NewGuid(),
            Id = 1,
            OrganisationId = Guid.NewGuid(),
            ReprocessingSiteAddress = new()
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                TownCity = "Town",
                County = "County",
                Country = "Country",
                GridReference = "T12345",
                PostCode = "postcode"
            },
            LegalDocumentAddress = new()
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                TownCity = "Town",
                County = "County",
                Country = "Country",
                GridReference = "T12345",
                PostCode = "postcode"
            },
            BusinessAddress = new()
            {
                AddressLine1 = "address line 1",
                AddressLine2 = "address line 2",
                TownCity = "Town",
                County = "County",
                Country = "Country",
                GridReference = "T12345",
                PostCode = "postcode"
            }
        };

        var expectedResult = new OkObjectResult(registration);

        // Expectations
        _registrationServiceMock
            .Setup(s => s.GetRegistrationByOrganisationAsync(1, organisationId))
            .ReturnsAsync(registration);

        // Act
        var result = await _controller.GetRegistrationByOrganisation(1, organisationId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestMethod]
    public async Task GetRegistrationByOrganisation_NoRegistrationFound_ShouldReturnNotFoundResult()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        
        var expectedResult = new NotFoundResult();

        // Expectations
        _registrationServiceMock
            .Setup(s => s.GetRegistrationByOrganisationAsync(1, organisationId))
            .ReturnsAsync((RegistrationDto?)null);

        // Act
        var result = await _controller.GetRegistrationByOrganisation(1, organisationId);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    } 

    [TestMethod]
    public async Task RegistrationOverview_ShouldReturnNoContentResult()
    {
        // Arrange
        var registrationId = 1;
        var request = new RegistrationOverviewDto { OrganisationName = "Org Name",Regulator = "UK"};

        // Act
        var result = await _controller.RegistrationOverview(registrationId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _registrationServiceMock.Verify(s => s.GetRegistrationOverviewAsync(registrationId), Times.Once);
    }

    [TestMethod]
    public async Task RegistrationOverview_ShouldReturnContentResult()
    {
        // Arrange
        var registrationId = 1;
        var request = new RegistrationOverviewDto { OrganisationName = "Org Name", Regulator = "UK" };
        _registrationServiceMock.Setup(r => r.GetRegistrationOverviewAsync(registrationId)).ReturnsAsync(request);

        // Act
        var result = await _controller.RegistrationOverview(registrationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>(); 
        _registrationServiceMock.Verify(s => s.GetRegistrationOverviewAsync(registrationId), Times.Once);
    }
}
