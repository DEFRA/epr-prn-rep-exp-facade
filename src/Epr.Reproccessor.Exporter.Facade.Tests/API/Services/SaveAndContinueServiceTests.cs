
using AutoFixture;
using AutoFixture.AutoMoq;
using Epr.Reproccessor.Exporter.Facade.Api.Models;
using Epr.Reproccessor.Exporter.Facade.Api.Services;
using Epr.Reproccessor.Exporter.Facade.App.Config;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace Epr.Reproccessor.Exporter.Facade.Tests.API.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SaveAndContinueServiceTests
    {
        private const string BaseAddress = "http://localhost";
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();
        private readonly NullLogger<SaveAndContinueService> _logger = new();
        private Mock<IOptions<PrnBackendServiceApiConfig>> _options = new();

        [TestInitialize]
        public void SetUp()
        {
            _options.Setup(x => x.Value).Returns(new PrnBackendServiceApiConfig()
            {
                BaseUrl = BaseAddress,
                Endpoints = new PrnBackendServiceEndpoint()
            });
        }

        [TestMethod]
        public async Task Save_UserJourney_ReturnsSuccessful()
        {
            // Arrange
            var apiResponse = _fixture
                .Build<HttpResponseMessage>()
                .With(x => x.StatusCode, HttpStatusCode.OK)
                .Create();

            var expectedUrl =
                $"{BaseAddress}/{_options.Object.Value.Endpoints.SaveAndContinueSaveUri}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x => x.RequestUri != null && x.RequestUri.ToString() == expectedUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(apiResponse)
                .Verifiable();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(BaseAddress);

            var sut = new SaveAndContinueService(httpClient, _logger, _options.Object);

            // Act
            var response = await sut.AddAsync(new SaveAndContinueRequest());

            // Assert
            response.Should().BeEquivalentTo(apiResponse);
        }

        [TestMethod]
        public async Task Save_UserJourney_ThrowsException()
        {
            // Arrange
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(BaseAddress);

            var sut = new SaveAndContinueService(httpClient, _logger, _options.Object);

            // Act
            Func<Task> act = () => sut.AddAsync(new SaveAndContinueRequest());

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [TestMethod]
        public async Task GetLatest_ShouldReturnSuccessfulResponse()
        {
            // Arrange
            var apiResponse = _fixture.Create<SaveAndContinueResponse>();
            var registrationId = 1;
            var area = "Registration";
            var controller = "Controller";

            var expectedUrl =
                $"{BaseAddress}/{_options.Object.Value.Endpoints.SaveAndContinueGetLatestUri}/{registrationId}/{area}/{controller}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x => x.RequestUri != null && x.RequestUri.ToString() == expectedUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(apiResponse))
                }).Verifiable();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(BaseAddress);

            var sut = new SaveAndContinueService(httpClient, _logger, _options.Object);

            // Act
            var response = await sut.GetLatestAsync(registrationId, controller, area);

            // Assert
            response.Should().BeEquivalentTo(apiResponse);
        }

        [TestMethod]
        public async Task GetLatest_ThrowsException()
        {
            // Arrange
            var registrationId = 1;
            var area = "Registration";
            var controller = "Controller";

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(BaseAddress);

            var sut = new SaveAndContinueService(httpClient, _logger, _options.Object);

            // Act
            Func<Task> act = () => sut.GetLatestAsync(registrationId, controller, area);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
