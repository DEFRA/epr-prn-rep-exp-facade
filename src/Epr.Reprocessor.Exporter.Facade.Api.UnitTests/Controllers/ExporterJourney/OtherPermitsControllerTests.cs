﻿using Epr.Reprocessor.Exporter.Facade.Api.Controllers.ExporterJourney;
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
			var registrationId = default(Guid);
			var otherPermitDto = new CarrierBrokerDealerPermitsDto { RegistrationId = registrationId };
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
			var registrationId = default(Guid);
			_serviceMock.Setup(s => s.Get(registrationId))
				.ReturnsAsync((CarrierBrokerDealerPermitsDto?)null);

			// Act
			var result = await _controller.Get(registrationId);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
			_serviceMock.Verify(s => s.Get(registrationId), Times.Once);
		}

		[TestMethod]
		public async Task Put_WithUpdatedOtherPermits_ShouldReturnNoContentResult()
		{
			// Arrange
			var registrationId = default(Guid);
			var request = new CarrierBrokerDealerPermitsDto();

			// Act
			var result = await _controller.Put(registrationId, request);

			// Assert
			result.Should().BeOfType<AcceptedResult>();
			_serviceMock.Verify(s => s.Update(registrationId, request), Times.Once);
		}
	}
}
