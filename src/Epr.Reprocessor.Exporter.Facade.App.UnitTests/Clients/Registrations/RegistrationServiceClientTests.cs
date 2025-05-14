using System.Net;
using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Constants;
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
                UpdateSiteAddressAndContactDetails = $"api/v{0}/{ApiUrls.UpdateSiteAddressAndContactDetails}",
            }
        });

        _client = new RegistrationServiceClient(httpClient, _mockOptions.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task UpdateSiteAddressAndContactDetails_ShouldReturnExpectedResult()
    {
        // Arrange
        var requestDto = _fixture.Create<UpdateSiteAddressAndContactDetailsDto>();

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.ToString().Contains(ApiUrls.UpdateSiteAddressAndContactDetails)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage 
            { 
                                StatusCode = HttpStatusCode.OK, 
                                Content = new StringContent("true") 
            });

        // Act
        var result = await _client.UpdateSiteAddressAndContactDetails(requestDto);

        // Assert
        result.Should().BeTrue();
    }

}
