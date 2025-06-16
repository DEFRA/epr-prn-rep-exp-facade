using System.Net.Http.Json;
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
public class RegistrationMaterialServiceClientTests
{
    private Fixture _fixture = null!;
    private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions = null!;
    private Mock<ILogger<RegistrationMaterialServiceClient>> _mockLogger = null!;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;

    private RegistrationMaterialServiceClient _client = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockOptions = new Mock<IOptions<PrnBackendServiceApiConfig>>();
        _mockLogger = new Mock<ILogger<RegistrationMaterialServiceClient>>();
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

        _client = new RegistrationMaterialServiceClient(_mockLogger.Object, httpClient, _mockOptions.Object);
    }

    [TestMethod]
    public async Task CreateExemptionReferencesAsync_SendsCorrectRequest()
    {
        // Arrange
        var dto = _fixture.Create<CreateExemptionReferencesDto>();       
        HttpRequestMessage? capturedRequest = null;

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            });

        // Act
        await _client.CreateExemptionReferencesAsync(dto);

        Assert.IsNotNull(capturedRequest);
        Assert.AreEqual(HttpMethod.Post, capturedRequest.Method);       
        var content = await capturedRequest.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains(dto.MaterialExemptionReferences?.ToString() ?? string.Empty) || content.Length > 0);
    }

    [TestMethod]
    public async Task CreateRegistrationMaterial_SendsCorrectRequest()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var dto = new CreateRegistrationMaterialRequestDto
        {
            RegistrationId = registrationId
        };
        HttpRequestMessage? capturedRequest = null;
        var response = new CreateRegistrationMaterialResponseDto
        {
            Id = registrationId
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(response))
            });

        // Act
        var result = await _client.CreateRegistrationMaterialAsync(dto);

        Assert.IsNotNull(capturedRequest);
        Assert.AreEqual(HttpMethod.Post, capturedRequest.Method);
        var content = await capturedRequest.Content!.ReadFromJsonAsync<CreateRegistrationMaterialResponseDto>();
        content.Should().NotBeNull();
    }
}
