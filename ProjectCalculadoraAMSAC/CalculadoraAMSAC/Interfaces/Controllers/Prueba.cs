using Microsoft.AspNetCore.Mvc;

namespace ProjectCalculadoraAMSAC.CalculadoraAMSAC.Interfaces.Controllers
{
    [ApiController]
    [Route("api/prueba")]
    public class PruebaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hola Mundo");
        }
    }
}