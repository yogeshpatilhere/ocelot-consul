using Microsoft.AspNetCore.Mvc;

namespace ConsulTry.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("GetTestData1")]
        public string GetTestData1()
        {
            return "GetTestData1";
        }
    }
}
