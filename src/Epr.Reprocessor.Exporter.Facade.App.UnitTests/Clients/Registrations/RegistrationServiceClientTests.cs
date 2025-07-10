using System.Net;
using System.Text.Json;
using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models;
using Epr.Reprocessor.Exporter.Facade.App.Enums;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Clients.Registrations;

[TestClass]

public class RegistrationServiceClientTests
{
    private Fixture _fixture = null!;
    private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions = null!;
    private Mock<ILogger<RegistrationServiceClient>> _mockLogger = null!;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private RegistrationServiceClient _client = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockOptions = new Mock<IOptions<PrnBackendServiceApiConfig>>();
        _mockLogger = new Mock<ILogger<RegistrationServiceClient>>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object) 
        {
            BaseAddress = new Uri("https://mock-api.com/") 
        };

        _mockOptions.Setup(opt => opt.Value).Returns(new PrnBackendServiceApiConfig
        {
            BaseUrl = "https://mock-api.com",
            Timeout = 30,
            ClientId = "test-client",
            ApiVersion = 1,
            ServiceRetryCount = 3,
            Endpoints = new PrnServiceApiConfigEndpoints
            {
                
            }
        });

        _client = new RegistrationServiceClient(httpClient, _mockOptions.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task GetRegistrationOverviewAsync_ShouldReturnOverview_WhenExists()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var overviewDto = new RegistrationOverviewDto
        {
            Id = 1,
            OrganisationName = "Test Org",
            Regulator = "Test Regulator",
            OrganisationType = ApplicationOrganisationType.Reprocessor,
            Tasks = new List<RegistrationTaskDto>(),
            Materials = new List<RegistrationMaterialDto>()
        };

        // Use the same URL format as the implementation
        var url = string.Format("api/v{0}/registrations/{1}", 1, registrationId);

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(overviewDto, JsonSerializerOptions))
            });

        // Act
        var result = await _client.GetRegistrationOverviewAsync(registrationId);

        // Assert
        result.Should().BeEquivalentTo(overviewDto);
    }

    [TestMethod]
    public async Task GetRegistrationOverviewAsync_ShouldThrowException_WhenNotFound()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var url = $"api/v1/registrations/{registrationId}/overview";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        var act = async () => await _client.GetRegistrationOverviewAsync(registrationId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task GetRegistrationOverviewAsync_ShouldThrowException_OnServerError()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var url = $"api/v1/registrations/{registrationId}/overview";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act
        var act = async () => await _client.GetRegistrationOverviewAsync(registrationId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task UpdateSiteAddressAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationSiteAddressDto>();
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.ToString().Contains("siteAddress")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage 
            { 
                                StatusCode = HttpStatusCode.OK, 
                                Content = new StringContent("true") 
            });

        // Act
        var result = await _client.UpdateSiteAddressAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task UpdateTaskStatusAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationTaskStatusDto>();
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.ToString().Contains("taskStatus")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("true")
            });

        // Act
        var result = await _client.UpdateRegistrationTaskStatusAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task UpdateApplicationTaskStatusAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationTaskStatusDto>();
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.ToString().Contains("applicationTaskStatus")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("true")
            });

        // Act
        var result = await _client.UpdateApplicationRegistrationTaskStatusAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetRegistrationByOrganisationAsync_Exists_ReturnDto()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var registrationDto = new RegistrationDto
        {
            Id = registrationId,
            ApplicationTypeId = 2,
            OrganisationId = organisationId
        };

        var url = $"api/v1/registrations/1/organisations/{organisationId}";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(registrationDto, JsonSerializerOptions))
            });

        // Act
        var result = await _client.GetRegistrationByOrganisationAsync(1, organisationId);

        // Assert
        result.Should().BeEquivalentTo(registrationDto);
    }

    [TestMethod]
    public async Task GetRegistrationByOrganisationAsync_NotFound_ShouldReturnNull()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
      
        var url = $"api/v1/registrations/1/organisations/{organisationId}";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        var result = await _client.GetRegistrationByOrganisationAsync(1, organisationId);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetRegistrationByOrganisationAsync_SomeOtherError_ShouldThrowException()
    {
        // Arrange
        var organisationId = Guid.NewGuid();

        var url = $"api/v1/registrations/1/organisations/{organisationId}";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act
        var act = async () => await _client.GetRegistrationByOrganisationAsync(1, organisationId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var requestDto = _fixture.Create<UpdateRegistrationDto>();
        var url = $"api/v1/registrations/{registrationId}/update";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("true")
            });

        // Act
        var result = await _client.UpdateAsync(registrationId, requestDto);

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewByOrgIdAsync_RegistrationsFound_ShouldReturnExpectedResult()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registrationsOverview = new List<RegistrationsOverviewDto>
    {
        new RegistrationsOverviewDto
        {
            RegistrationId = Guid.NewGuid(),
            RegistrationMaterialId = 1,
            MaterialId = 101,
            Material = "Plastic",
            MaterialCode = "PL001",
            ApplicationTypeId = 2,
            ApplicationType = "Recycling",
            RegistrationStatus = 1,
            AccreditationStatus = 2,
            ReprocessingSiteId = 123,
            ReprocessingSiteAddress = new AddressDto
            {
                AddressLine1 = "123 Recycling Lane",
                AddressLine2 = "Unit 5",
                TownCity = "Recycle City",
                County = "Green County",
                Country = "EcoLand",
                GridReference = "GR12345",
                PostCode = "RC1 2PL"
            },
            RegistrationYear = 2023,
            AccreditationYear = 2022
        }
    };
        // Correct the URL to match the actual format
        var url = $"api/v1/registrations/{organisationId}/overview";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().Contains(url)), // Match the correct URL
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(registrationsOverview, JsonSerializerOptions))
            });
        // Act
        var result = await _client.GetRegistrationsOverviewByOrgIdAsync(organisationId);
        // Assert
        result.Should().BeEquivalentTo(registrationsOverview);
        // Verify the request was made with the correct HTTP method and URL
        _mockHttpMessageHandler.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString().Contains(url)), // Match the correct URL
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewByOrgIdAsync_NotFound_ShouldReturnNull()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        // Correct the URL to match the actual format
        var url = $"api/v1/registrations/{organisationId}/overview";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().Contains(url)), // Match the correct URL
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound // Simulate a 404 response
            });
        // Act
        var result = await _client.GetRegistrationsOverviewByOrgIdAsync(organisationId);
        // Assert
        result.Should().BeNull();
        // Verify the request was made with the correct HTTP method and URL
        _mockHttpMessageHandler.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString().Contains(url)), // Match the correct URL
                ItExpr.IsAny<CancellationToken>()
            );
    }


    [TestMethod]
    public async Task GetRegistrationsOverviewByOrgIdAsync_OtherError_ShouldThrowException()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var url = $"api/v1/registrations/overview/{organisationId}";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });
        // Act
        var act = async () => await _client.GetRegistrationsOverviewByOrgIdAsync(organisationId);
        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [TestMethod]
    public async Task GetRegistrationsOverviewByOrgIdAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registrationsOverview = new List<RegistrationsOverviewDto>
    {
        new RegistrationsOverviewDto
        {
            RegistrationId = Guid.NewGuid(),
            RegistrationMaterialId = 1,
            MaterialId = 101,
            Material = "Plastic",
            MaterialCode = "PL001",
            ApplicationTypeId = 2,
            ApplicationType = "Recycling",
            RegistrationStatus = 1,
            AccreditationStatus = 2,
            ReprocessingSiteId = 123,
            ReprocessingSiteAddress = new AddressDto
            {
                AddressLine1 = "123 Recycling Lane",
                AddressLine2 = "Unit 5",
                TownCity = "Recycle City",
                County = "Green County",
                Country = "EcoLand",
                GridReference = "GR12345",
                PostCode = "RC1 2PL"
            },
            RegistrationYear = 2023,
            AccreditationYear = 2022
        }
    };
        // Correct the URL to match the actual format
        var url = $"api/v1/registrations/{organisationId}/overview";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().Contains(url)), // Match the correct URL
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(registrationsOverview, JsonSerializerOptions))
            });
        // Act
        var result = await _client.GetRegistrationsOverviewByOrgIdAsync(organisationId);
        // Assert
        result.Should().BeEquivalentTo(registrationsOverview);
        // Verify the request was made with the correct HTTP method and URL
        _mockHttpMessageHandler.Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString().Contains(url)), // Match the correct URL
                ItExpr.IsAny<CancellationToken>()
            );
    }


}