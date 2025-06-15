using System.Net;
using System.Text.Json;
using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Config;
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
    public async Task GetRegistrationByOrganisationAsync_Exists_ReturnDto()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var registrationDto = new RegistrationDto
        {
            Id = 1,
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
                Content = new StringContent(JsonSerializer.Serialize(registrationDto, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }))
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
        var url = "api/v1/registrations/1/update";
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
}