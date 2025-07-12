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
public class OverseasAccreditationSiteServiceClientTests
{
    private Fixture _fixture;
    private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private OverseasAccreditationSiteServiceClient _client;
    private readonly JsonSerializerOptions _camelCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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
                OverseasAccreditationSiteGet = "api/v{0}/OverseasAccreditationSite/{1}",
                OverseasAccreditationSitePost = "api/v{0}/OverseasAccreditationSite/{1}"
            }
        };
        _mockOptions.Setup(x => x.Value).Returns(config);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://mock-api.com/")
        };

        _client = new OverseasAccreditationSiteServiceClient(httpClient, _mockOptions.Object);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ShouldReturnExpectedSiteDtos()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var expected = _fixture.Create<List<OverseasAccreditationSiteDto>>();
        var url = $"api/v1/OverseasAccreditationSite/{accreditationId}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.PathAndQuery.EndsWith(url)
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expected, _camelCaseJsonSerializerOptions))
            });

        // Act
        var result = await _client.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task PostByAccreditationId_ShouldSendPostRequest()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = _fixture.Create<OverseasAccreditationSiteDto>();
        var url = $"api/v1/OverseasAccreditationSite/{accreditationId}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.PathAndQuery.EndsWith(url)
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

        // Act
        await _client.PostByAccreditationId(accreditationId, request);

        // Assert
        _mockHttpMessageHandler.VerifyAll();
    }
}
