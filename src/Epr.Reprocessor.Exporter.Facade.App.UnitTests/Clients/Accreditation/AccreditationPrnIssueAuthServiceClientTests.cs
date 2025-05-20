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
public class AccreditationPrnIssueAuthServiceClientTests
{
    private Fixture _fixture;
    private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private AccreditationPrnIssueAuthServiceClient _client;

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
                AccreditationPrnIssueAuthGet = "api/v{0}/accreditationprnissueauth/{1}",
                AccreditationPrnIssueAuthPost = "api/v{0}/accreditationprnissueauth/{1}"
            }
        };
        _mockOptions.Setup(x => x.Value).Returns(config);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://mock-api.com/")
        };

        _client = new AccreditationPrnIssueAuthServiceClient(httpClient, _mockOptions.Object);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ShouldReturnExpectedDtos()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var expected = _fixture.Create<List<AccreditationPrnIssueAuthDto>>();
        var json = SerializeCamelCase(expected);
        var url = $"api/v1/accreditationprnissueauth/{accreditationId}";

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
        var result = await _client.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task ReplaceAllByAccreditationId_ShouldSendPostRequest()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = _fixture.Create<List<AccreditationPrnIssueAuthRequestDto>>();
        var url = $"api/v1/accreditationprnissueauth/{accreditationId}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.PathAndQuery.EndsWith(url)
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            });

        // Act
        await _client.ReplaceAllByAccreditationId(accreditationId, request);

        // Assert
        _mockHttpMessageHandler.VerifyAll();
    }

    private static string SerializeCamelCase<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
