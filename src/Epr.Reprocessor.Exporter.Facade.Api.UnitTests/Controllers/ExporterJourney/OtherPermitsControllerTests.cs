using Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Models.Accreditations;
using Epr.Reprocessor.Exporter.Facade.App.Models.ExporterJourney;
using Epr.Reprocessor.Exporter.Facade.App.Services.ExporterJourney.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;

namespace Epr.Reprocessor.Exporter.Facade.Api.UnitTests.Controllers.ExporterJourney
{
	[TestClass]
	public class OtherPermitsControllerTests
	{
		private Mock<IOtherPermitsService> _serviceMock;
		private Mock<ILogger<OtherPermitsController>> _loggerMock;
		private OtherPermitsController _controller;

		[TestInitialize]
		public void SetUp()
		{
			_serviceMock = new Mock<IOtherPermitsService>();
			_loggerMock = new Mock<ILogger<OtherPermitsController>>();
			_controller = new OtherPermitsController(_serviceMock.Object, _loggerMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ShouldThrowArgumentNullException_WhenServiceIsNull()
		{
			// Act
			var controller = new OtherPermitsController(null, _loggerMock.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
		{
			// Act
			var controller = new OtherPermitsController(_serviceMock.Object, null);
		}

		[TestMethod]
		public async Task Get_ShouldReturnOk_WhenDataObjectWithRegistrationIdExists()
		{
			// Arrange
			var registrationId = 1;
			var otherPermitDto = new OtherPermitsDto { RegistrationId = registrationId };
			_serviceMock.Setup(s => s.Get(registrationId))
				.ReturnsAsync(otherPermitDto);

			// Act
			var result = await _controller.Get(registrationId);

			// Assert
			result.Should().BeOfType<OkObjectResult>();
			var okResult = result as OkObjectResult;
			okResult!.Value.Should().Be(otherPermitDto);
			_serviceMock.Verify(s => s.Get(registrationId), Times.Once);
		}

		[TestMethod]
		public async Task Get_ShouldReturnNotFound_WhenDataObjectDoesNotExist()
		{
			// Arrange
			var registrationId = 1;
			_serviceMock.Setup(s => s.Get(registrationId))
				.ReturnsAsync((OtherPermitsDto?)null);

			// Act
			var result = await _controller.Get(registrationId);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
			_serviceMock.Verify(s => s.Get(registrationId), Times.Once);
		}


		[TestMethod]
		public async Task Post_ShouldReturnOk_WithCreatedOtherPermits()
		{
			// Arrange
			var registrationId = 1;
			var request = new OtherPermitsDto { RegistrationId = registrationId };
			var response = new OtherPermitsDto { RegistrationId = request.RegistrationId, Id = 1 };
			_serviceMock.Setup(s => s.Create(request))
				.ReturnsAsync(response.Id);

			// Act
			var result = await _controller.Post(request);

			// Assert
			result.Should().BeOfType<OkObjectResult>();
			var okResult = result as OkObjectResult;
			result.Equals(response.Id);
			_serviceMock.Verify(s => s.Create(request), Times.Once);
		}

		[TestMethod]
		public async Task Put_WithUpdatedOtherPermits_ShouldReturnNoContentResult()
		{
			// Arrange
			var registrationId = 1;
			var request = new OtherPermitsDto();

			// Act
			var result = await _controller.Put(registrationId, request);

			// Assert
			result.Should().BeOfType<AcceptedResult>();
			_serviceMock.Verify(s => s.Update(registrationId, request), Times.Once);
		}
	}
}
