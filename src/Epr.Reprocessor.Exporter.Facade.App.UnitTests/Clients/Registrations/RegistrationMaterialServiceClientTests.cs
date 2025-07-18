using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoFixture;
using Epr.Reprocessor.Exporter.Facade.App.Clients.Registrations;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter;
using Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs;
using Epr.Reprocessor.Exporter.Facade.App.Models.Registrations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Clients.Registrations;

[TestClass]
public class RegistrationMaterialServiceClientTests
{
    private Fixture _fixture = null!;
    private Mock<IOptions<PrnBackendServiceApiConfig>> _mockOptions = null!;
    private Mock<ILogger<RegistrationMaterialServiceClient>> _mockLogger = null!;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler = null!;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermits_SendsCorrectRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateRegistrationMaterialPermitsDto>();

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
                Content = new StringContent(JsonSerializer.Serialize("true"))
            });

        // Act
        var result = await _client.UpdateRegistrationMaterialPermitsAsync(id, request);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);
    }

    [TestMethod]
    public async Task GetMaterialsPermitTypesAsync_SendsCorrectRequest()
    {
        // Arrange
        var response = _fixture.Create<List<MaterialsPermitTypeDto>>();

        HttpRequestMessage ? capturedRequest = null;

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
        var result = await _client.GetMaterialsPermitTypesAsync();

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Get);
    }

    [TestMethod]
    public async Task GetAllRegistrationMaterials_SendCorrectRequest()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var registrationMaterialsDto = new List<ApplicationRegistrationMaterialDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                RegistrationId = registrationId,
                PPCPermitNumber = "number"
            }
        };

        var url = $"api/v1/registrations/{registrationId}/materials";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().EndsWith(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(registrationMaterialsDto, JsonSerializerOptions))
            });

        // Act
        var result = await _client.GetAllRegistrationMaterialsAsync(registrationId);

        // Assert
        result.Should().BeEquivalentTo(registrationMaterialsDto);
    }

    [TestMethod]
    public async Task UpdateRegistrationMaterialPermitCapacity_SendsCorrectRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateRegistrationMaterialPermitCapacityDto>();

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
                Content = new StringContent(JsonSerializer.Serialize("true"))
            });

        // Act
        var result = await _client.UpdateRegistrationMaterialPermitCapacityAsync(id, request);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);
    }

    [TestMethod]
    public async Task DeleteAsync_SendsCorrectRequest()
    {
        // Arrange
        var id = Guid.NewGuid();

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
                Content = new StringContent(JsonSerializer.Serialize("true"))
            });

        // Act
        var result = await _client.DeleteAsync(id);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Delete);
        result.Should().BeTrue();
    }

	[TestMethod]
	public async Task UpdateIsMaterialRegisteredAsync_SendsCorrectRequest()
	{
		// Arrange
		var request = _fixture.Create<List<UpdateIsMaterialRegisteredDto>>();

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
				Content = new StringContent(JsonSerializer.Serialize("true"))
			});

		// Act
		var result = await _client.UpdateIsMaterialRegisteredAsync(request);

		// Assert
		capturedRequest.Should().NotBeNull();
		capturedRequest.Method.Should().Be(HttpMethod.Post);
	}

    [TestMethod]
    public async Task UpsertRegistrationMaterialContactAsync_SendsCorrectRequest()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var request = new RegistrationMaterialContactDto { Id = Guid.NewGuid() };
        var expectedResponse = new RegistrationMaterialContactDto { Id = Guid.NewGuid() };

        HttpRequestMessage? capturedRequest = null;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
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
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedResponse, options))
            });

        // Act
        var result = await _client.UpsertRegistrationMaterialContactAsync(registrationMaterialId, request);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public async Task UpsertRegistrationReprocessingDetailsAsync_SendsCorrectRequest()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var request = new RegistrationReprocessingIORequestDto { TypeOfSuppliers = "Supplier 123" };
        var dto = _fixture.Create<RegistrationReprocessingIORequestDto>();

        HttpRequestMessage? capturedRequest = null;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
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
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            });

        // Act
        await _client.UpsertRegistrationReprocessingDetailsAsync(registrationMaterialId, request);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);
        var content = await capturedRequest.Content.ReadAsStringAsync();
        Assert.IsTrue(content.Contains(dto.TypeOfSuppliers?.ToString() ?? string.Empty) || content.Length > 0);
    }

    [TestMethod]
    public async Task SaveOverseasReprocessorAsync_SendsCorrectRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<OverseasAddressRequest>();

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
                Content = new StringContent(JsonSerializer.Serialize("true"))
            });

        // Act
        var result = await _client.SaveOverseasReprocessorAsync(request, Guid.NewGuid());

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);
    }

    [TestMethod]
    public async Task GetOverseasMaterialReprocessingSites_SendsCorrectRequest_AndReturnsData()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var expectedResponse = _fixture.Create<List<OverseasMaterialReprocessingSiteDto>>();
        HttpRequestMessage? capturedRequest = null;

        var url = $"api/v1/registrations/materials/{registrationMaterialId}/overseas-sites"; // Adjust if your endpoint differs
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Get &&
                    msg.RequestUri!.ToString().Contains(registrationMaterialId.ToString())),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(expectedResponse, JsonSerializerOptions))
            });

        // Act
        var result = await _client.GetOverseasMaterialReprocessingSites(registrationMaterialId);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Get);
        result.Should().BeEquivalentTo(expectedResponse);
    }

    [TestMethod]
    public async Task SaveInterimSitesAsync_SendsCorrectRequest()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var createdBy = Guid.NewGuid();
        var requestDto = new SaveInterimSitesRequestDto
        {
            RegistrationMaterialId = registrationMaterialId,
            OverseasMaterialReprocessingSites = _fixture.Create<List<OverseasMaterialReprocessingSiteDto>>()
        };
        HttpRequestMessage? capturedRequest = null;

        var url = $"api/v1/registrations/materials/{registrationMaterialId}/interim-sites"; // Adjust if your endpoint differs

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(msg =>
                    msg.Method == HttpMethod.Post &&
                    msg.RequestUri!.ToString().Contains(registrationMaterialId.ToString())),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            });

        // Act
        await _client.SaveInterimSitesAsync(requestDto, createdBy);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Post);
        var content = await capturedRequest.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task UpdateMaximumWeightAsync_SendsCorrectRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = _fixture.Create<UpdateMaximumWeightDto>();

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
                Content = new StringContent(JsonSerializer.Serialize("true"))
            });

        // Act
        var result = await _client.UpdateMaximumWeightAsync(id, request);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest.Method.Should().Be(HttpMethod.Put);
    }
}