using Microsoft.AspNetCore.Mvc;

namespace MiAPI.Controllers
{
    [Route("api/[Controller]")]
    public class ParImparController : ControllerBase
    {
        [HttpGet("{numero}")]
        public IActionResult EsParImpar(int numero)
        {
            var resultado = (numero % 2 == 0) ? "par" : "impar";
            return Ok(new {Numero = numero, Resultado = resultado});
        }
    }
}