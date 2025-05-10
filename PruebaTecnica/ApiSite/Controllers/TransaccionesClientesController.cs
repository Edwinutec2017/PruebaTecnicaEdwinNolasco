using Microsoft.AspNetCore.Mvc;

namespace ApiSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransaccionesClientesController : ControllerBase
    {


        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
            return "Pruieba";
        }
    }
}
