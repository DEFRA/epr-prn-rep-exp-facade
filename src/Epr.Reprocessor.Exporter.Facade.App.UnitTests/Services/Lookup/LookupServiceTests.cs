using Epr.Reprocessor.Exporter.Facade.App.Clients.Lookup;
using Epr.Reprocessor.Exporter.Facade.App.Services.Lookup;
using FluentAssertions;
using Moq;

namespace Epr.Reprocessor.Exporter.Facade.App.UnitTests.Services.Lookup;

[TestClass]
public class LookupServiceTests
{
    private Mock<ILookupServiceClient> _lookupServiceClientMock = null!;
    private LookupService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _lookupServiceClientMock = new Mock<ILookupServiceClient>();
        _service = new LookupService(_lookupServiceClientMock.Object);
    }

    [TestMethod]
    public async Task GetCountries_ReturnsCountries()
    {
        // Arrange
        var expected = new List<string> { "UK", "France", "Germany" };
        _lookupServiceClientMock.Setup(x => x.GetCountries()).ReturnsAsync(expected);

        // Act
        var result = await _service.GetCountries();

        // Assert
        result.Should().BeEquivalentTo(expected);
        _lookupServiceClientMock.Verify(x => x.GetCountries(), Times.Once);
    }

    [TestMethod]
    public async Task GetCountries_ReturnsEmptyList_WhenNoCountries()
    {
        // Arrange
        var expected = new List<string>();
        _lookupServiceClientMock.Setup(x => x.GetCountries()).ReturnsAsync(expected);

        // Act
        var result = await _service.GetCountries();

        // Assert
        result.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetCountries_ThrowsException_WhenClientThrows()
    {
        // Arrange
        _lookupServiceClientMock.Setup(x => x.GetCountries()).ThrowsAsync(new System.Exception("Test exception"));

        // Act
        var act = async () => await _service.GetCountries();

        // Assert
        await act.Should().ThrowAsync<System.Exception>().WithMessage("Test exception");
    }
}
