using Microsoft.AspNetCore.Mvc;

namespace Epr.Reproccessor.Exporter.Facade.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDefault")]
        public string Get()
        {
            return "Default method";
        }
    }
}