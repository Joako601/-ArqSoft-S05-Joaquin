using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CalculadoraController : ControllerBase
	{
		[HttpGet("sumar")]

		public IActionResult Sumar([FromQuery] double a, [FromQuery] double b)
		{
			return Ok(new { operacion = "suma", a = a, b = b, resultado = a + b });
		}

		[HttpGet("restar")]
		public IActionResult Restar([FromQuery] double a, [FromQuery] double b)
		{
			return Ok(new { operacion = "resta", a = a, b = b, resultado = a - b });
		}

		[HttpGet("multiplicar")]
		public IActionResult Multiplicar([FromQuery] double a, [FromQuery] double b)
		{
			return Ok(new { operacion = "multiplicacion", a = a, b = b, resultado = a * b });
		}

		[HttpGet("dividir")]
		public IActionResult Dividir([FromQuery] double a, [FromQuery] double b)
		{
			if (b == 0)
			{
				return BadRequest(new { error = "No se puede dividir entre cero." });
			}

			return Ok(new { operacion = "division", a = a, b = b, resultado = a / b });
		}
	}
}