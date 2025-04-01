
using AutoFixture;
using AutoFixture.AutoMoq;
using Epr.Reproccessor.Exporter.Facade.Api.Models;
using Epr.Reproccessor.Exporter.Facade.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Epr.Reproccessor.Exporter.Facade.Tests.API.Services
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SaveAndContinueServiceTests
    {
        private const string SaveAndContinueUri = "api/v1.0/saveandcontinue/save";
        private const string BaseAddress = "http://localhost";
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();
        private readonly NullLogger<SaveAndContinueService> _logger = new();
        private readonly IConfiguration _configuration = GetConfig();

        private static IConfiguration GetConfig()
        {
            var config = new Dictionary<string, string?>
        {
            {"PrnBackendServiceApiConfig:BaseUrl", BaseAddress},
        };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            return configuration;
        }

        [TestMethod]
        public async Task Save_User_Journey_ReturnsSuccessful()
        {
            // Arrange
            var apiResponse = _fixture
                .Build<HttpResponseMessage>()
                .With(x => x.StatusCode, HttpStatusCode.OK)
                .Create();

            var expectedUrl =
                $"{BaseAddress}/{SaveAndContinueUri}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(x => x.RequestUri != null && x.RequestUri.ToString() == expectedUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(apiResponse)
                .Verifiable();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(BaseAddress);

            var sut = new SaveAndContinueService(httpClient, _logger, _configuration);

            // Act
            var response = await sut.SaveAsync(new SaveAndContinueModel());

            // Assert
            response.Should().BeEquivalentTo(apiResponse);
        }

        [TestMethod]
        public async Task Save_User_Journey_ThrowsException()
        {
            // Arrange
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri(BaseAddress);

            var sut = new SaveAndContinueService(httpClient, _logger, _configuration);

            // Act
            Func<Task> act = () => sut.SaveAsync(new SaveAndContinueModel());

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
