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
                AccreditationGetOrCreate = "api/v{0}/accreditation/{1}/{2}/{3}",
                AccreditationGet = "api/v{0}/accreditation/{1}",
                AccreditationPost = "api/v{0}/accreditation",
                AccreditationFileUploadGet = "api/v{0}/accreditation/Files/{1}",
                AccreditationFileUploadsGet = "api/v{0}/accreditation/{1}/Files/{2}/{3}",
                AccreditationFileUploadPost = "api/v{0}/accreditation/{1}/Files",
                AccreditationFileUploadDelete = "api/v{0}/accreditation/{1}/Files/{2}"
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
    public async Task GetOrCreateAccreditation_ShouldReturn_AccreditationId()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;
        var json = SerializeCamelCase(accreditationId);
        var url = $"api/v1/accreditation/{organisationId}/{materialId}/{applicationTypeId}";

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
        var result = await _client.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId);

        // Assert
        result.Should().Be(accreditationId);
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

    [TestMethod]
    public async Task GetFileUploads_ShouldReturnExpectedList()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadTypeId = 1;
        var fileUploadStatusId = 2;
        var expected = _fixture.Create<List<AccreditationFileUploadDto>>();
        var json = SerializeCamelCase(expected);
        var url = $"api/v1/accreditation/{accreditationId}/Files/{fileUploadTypeId}/{fileUploadStatusId}";

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
        var result = await _client.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task GetFileUpload_ShouldReturnExpectedData()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var fileUploadTypeId = 1;
        var fileUploadStatusId = 2;
        var expected = _fixture.Create<AccreditationFileUploadDto>();
        var json = SerializeCamelCase(expected);
        var url = $"api/v1/accreditation/Files/{externalId}";

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
        var result = await _client.GetFileUpload(externalId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task GetFileUploads_ShouldReturnExpectedList()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileUploadTypeId = 1;
        var fileUploadStatusId = 2;
        var expected = _fixture.Create<List<AccreditationFileUploadDto>>();
        var json = SerializeCamelCase(expected);
        var url = $"api/v1/accreditation/{accreditationId}/Files/{fileUploadTypeId}/{fileUploadStatusId}";

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
        var result = await _client.GetFileUploads(accreditationId, fileUploadTypeId, fileUploadStatusId);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task UpsertFileUpload_ShouldReturnExpectedDto()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = _fixture.Create<AccreditationFileUploadDto>();
        var expected = _fixture.Create<AccreditationFileUploadDto>();
        var json = SerializeCamelCase(expected);
        var url = $"api/v1/accreditation/{accreditationId}/Files";

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
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            });

        // Act
        var result = await _client.UpsertFileUpload(accreditationId, request);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task DeleteFileUpload_ShouldSendDeleteRequest()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var fileId = Guid.NewGuid();
        var url = $"api/v1/accreditation/{accreditationId}/Files/{fileId}";

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Delete &&
                    msg.RequestUri!.PathAndQuery.EndsWith(url)
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        // Act
        await _client.DeleteFileUpload(accreditationId, fileId);

        // Assert
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(msg =>
                msg.Method == HttpMethod.Delete &&
                msg.RequestUri!.PathAndQuery.EndsWith(url)
            ),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    private static string SerializeCamelCase<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}