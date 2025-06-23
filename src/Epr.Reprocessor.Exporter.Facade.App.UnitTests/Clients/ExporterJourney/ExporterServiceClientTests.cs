using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using System.Net;
using Epr.Reprocessor.Exporter.Facade.App.Clients.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using System.Text.Json;
using FluentAssertions;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Clients.ExporterJourney
{
    [TestClass]
	public class ExporterServiceClientTests
	{
		private Fixture _fixture = null!;
		private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions = null!;
		private Mock<ILogger<ExporterServiceClient>> _mockLogger = null!;
		private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;

		private ExporterServiceClient _client = null!;

		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();
			_mockOptions = new Mock<IOptions<PrnBackendServiceApiConfig>>();
			_mockLogger = new Mock<ILogger<ExporterServiceClient>>();
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

			_client = new ExporterServiceClient(httpClient, _mockOptions.Object, _mockLogger.Object);
		}

		[TestMethod]
		public async Task SendGetRequest_ShouldReturnExpectedResult()
		{
			// Arrange
			var expected = _fixture.Create<CarrierBrokerDealerPermitsDto>();
			var json = SerializeCamelCase(expected);
			var url = $"api/v1/registration/1/other-permits";

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
			var result = await _client.SendGetRequest<CarrierBrokerDealerPermitsDto>(url);

			// Assert
			result.Should().BeEquivalentTo(expected);
		}

		[TestMethod]
		public async Task SendPostRequest_ShouldReturnExpectedId()
		{
			// Arrange
			var request = _fixture.Create<CarrierBrokerDealerPermitsDto>();
			var expected = Guid.NewGuid();
			var json = SerializeCamelCase(expected);
			var url = $"api/v1/registration/1/other-permits";

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
			var result = await _client.SendPostRequest<CarrierBrokerDealerPermitsDto>(url, request);

			// Assert
			result.Should().Be(expected);
		}

		[TestMethod]
		public async Task SendPutRequest_ShouldReturnSuccessfully()
		{
			// Arrange
			var request = _fixture.Create<CarrierBrokerDealerPermitsDto>();
			var expected = true;
			var json = SerializeCamelCase(request);
			var url = $"api/v1/registration/1/other-permits";

			_mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.Is<HttpRequestMessage>(msg =>
						msg.Method == HttpMethod.Put &&
						msg.RequestUri!.PathAndQuery.EndsWith(url)),
					ItExpr.IsAny<CancellationToken>()
				)
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(json)
				});

			// Act
			await _client.SendPutRequest<CarrierBrokerDealerPermitsDto>(url, request);
		}

		private static string SerializeCamelCase<T>(T obj)
		{
			return JsonSerializer.Serialize(obj, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});
		}
	}
}
