using System.Net;
using System.Text.Json;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Lookup;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Clients.Lookup;

[TestClass]
public class LookupServiceClientTests
{
    private PrnBackendServiceApiConfig _config = null!;
    private Mock<IOptions<PrnBackendServiceApiConfig>> _optionsMock = null!;
    private Mock<HttpMessageHandler> _httpMessageHandlerMock = null!;
    private HttpClient _httpClient = null!;
    private LookupServiceClient _client = null!;

    [TestInitialize]
    public void Setup()
    {
        _optionsMock = new Mock<IOptions<PrnBackendServiceApiConfig>>();
        _optionsMock.Setup(opt => opt.Value).Returns(new PrnBackendServiceApiConfig
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

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        _client = new LookupServiceClient(_httpClient, _optionsMock.Object);
    }

    [TestMethod]
    public async Task GetCountries_ReturnsCountries()
    {
        // Arrange
        var expected = new List<string> { "UK", "France", "Germany" };
        var responseContent = JsonSerializer.Serialize(expected);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent, System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString().EndsWith("api/v1/countries")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.GetCountries();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task GetCountries_ReturnsEmptyList_WhenNoCountries()
    {
        // Arrange
        var expected = new List<string>();
        var responseContent = JsonSerializer.Serialize(expected);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent, System.Text.Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString().EndsWith("api/v1/countries")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        // Act
        var result = await _client.GetCountries();

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetCountries_ThrowsException_WhenHttpFails()
    {
        // Arrange
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act
        var act = async () => await _client.GetCountries();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>().WithMessage("Network error");
    }
}
