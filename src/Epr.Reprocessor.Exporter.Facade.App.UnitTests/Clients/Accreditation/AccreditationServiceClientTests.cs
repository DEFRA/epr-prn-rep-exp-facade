using System.Net;
using System.Text.Json;
using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Accreditation;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Clients.Accreditation;

[TestClass]
public class AccreditationServiceClientTests
{
    private Fixture _fixture;
    private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private AccreditationServiceClient _client;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockOptions = new Mock<IOptions<PrnBackendServiceApiConfig>>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        var config = new PrnBackendServiceApiConfig
        {
            ApiVersion = 1,
            Endpoints = new PrnServiceApiConfigEndpoints
            {
                AccreditationGet = "api/v{0}/accreditation/{1}",
                AccreditationPost = "api/v{0}/accreditation"
            }
        };
        _mockOptions.Setup(x => x.Value).Returns(config);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://mock-api.com/")
        };

        _client = new AccreditationServiceClient(httpClient, _mockOptions.Object);
    }

    [TestMethod]
    public async Task GetAccreditationById_ShouldReturnExpectedDto()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var expected = _fixture.Create<AccreditationDto>();
        var json = SerializeCamelCase(expected);
        var url = $"api/v1/accreditation/{accreditationId}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.PathAndQuery.EndsWith(url)
                    ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

        // Act
        var result = await _client.GetAccreditationById(accreditationId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task UpsertAccreditation_ShouldReturnExpectedDto()
    {
        // Arrange
        var request = _fixture.Create<AccreditationRequestDto>();
        var expected = _fixture.Create<AccreditationDto>();
        var json = SerializeCamelCase(expected);
        var url = "api/v1/accreditation";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.PathAndQuery.EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

        // Act
        var result = await _client.UpsertAccreditation(request);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    private static string SerializeCamelCase<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}