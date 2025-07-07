using Epr.Reprocessor.Exporter.Facade.Api.Controllers.Lookup;
using Epr.Reprocessor.Exporter.Facade.App.Services.Lookup;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Controllers.Lookups;

[TestClass]
public class CountryControllerTests
{
    private Mock<ILookupService> _lookupServiceMock = null!;
    private Mock<ILogger<CountryController>> _loggerMock = null!;
    private CountryController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _lookupServiceMock = new Mock<ILookupService>();
        _loggerMock = new Mock<ILogger<CountryController>>();
        _controller = new CountryController(_lookupServiceMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetCountries_ReturnsOkWithCountries()
    {
        // Arrange
        var expectedCountries = new List<string> { "UK", "France", "Germany" };
        _lookupServiceMock.Setup(s => s.GetCountries()).ReturnsAsync(expectedCountries);

        // Act
        var result = await _controller.GetCountries();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedCountries);

        _lookupServiceMock.Verify(s => s.GetCountries(), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Attempting to get Countries")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }

    [TestMethod]
    public async Task GetCountries_ReturnsOkWithEmptyList_WhenNoCountries()
    {
        // Arrange
        var expectedCountries = new List<string>();
        _lookupServiceMock.Setup(s => s.GetCountries()).ReturnsAsync(expectedCountries);

        // Act
        var result = await _controller.GetCountries();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedCountries);
    }

    [TestMethod]
    public async Task GetCountries_ThrowsException_ReturnsException()
    {
        // Arrange
        _lookupServiceMock.Setup(s => s.GetCountries()).ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var act = async () => await _controller.GetCountries();

        // Assert
        await act.Should().ThrowAsync<System.Exception>().WithMessage("Test exception");
    }
}
