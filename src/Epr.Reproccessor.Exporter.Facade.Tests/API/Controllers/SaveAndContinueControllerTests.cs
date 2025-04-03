using Epr.Reproccessor.Exporter.Facade.Api.Controllers;
using Epr.Reproccessor.Exporter.Facade.Api.Models;
using Epr.Reproccessor.Exporter.Facade.Api.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Epr.Reproccessor.Exporter.Facade.Tests.API.Controllers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SaveAndContinueControllerTests
    {
        private SaveAndContinueController _systemUnderTest;
        private Mock<ISaveAndContinueService> _mockSaveAndContinueService;
        private Mock<ILogger<SaveAndContinueController>> _mockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockSaveAndContinueService = new Mock<ISaveAndContinueService>();
            _mockLogger = new Mock<ILogger<SaveAndContinueController>>();

            _systemUnderTest = new SaveAndContinueController(_mockSaveAndContinueService.Object,
                _mockLogger.Object);
        }

        [TestMethod]
        public async Task Create_ReturnsOk()
        {
            var model = new SaveAndContinueRequest() { RegistrationId = 1, Action = "Test", Controller = "Test", Area = "Area" };
            var result = await _systemUnderTest.Create(model) as OkResult;

            result.Should().BeOfType<OkResult>();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task Create_InternalServerError_ReturnsBadRequest()
        {
            _mockSaveAndContinueService.Setup(s => s.AddAsync(It.IsAny<SaveAndContinueRequest>())).Throws<ArgumentException>();

            var result = await _systemUnderTest.Create(null) as BadRequestResult;

            result.Should().BeOfType<BadRequestResult>();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task GetLatest_ReturnsOk()
        {
            var data = new SaveAndContinueResponse() { Id = 1, RegistrationId = 1, Area = "Area", Controller= "Controller", Action ="Action", CreatedOn = DateTime.UtcNow };
            var registrationId = 1;
            var area = "Registration";

            _mockSaveAndContinueService.Setup(x=>x.GetLatestAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(data);

            var result = await _systemUnderTest.GetLatest(registrationId, area) as OkObjectResult;

            result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }


        [TestMethod]
        public async Task GetLatest_InternalServerError_ReturnsBadRequest()
        {
            _mockSaveAndContinueService.Setup(x => x.GetLatestAsync(It.IsAny<int>(), It.IsAny<string>())).Throws<ArgumentException>();
            var registrationId = 1;
            var area = "Registration";

            var result = await _systemUnderTest.GetLatest(registrationId, area) as BadRequestResult;

            result.Should().BeOfType<BadRequestResult>();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
