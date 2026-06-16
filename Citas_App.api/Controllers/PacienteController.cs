using Citas_.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PacienteController : ControllerBase
	{

		private readonly PacienteService _service;

		public PacienteController(PacienteService service)
		{

			_service = service;

		}

		[HttpGet]
		public IActionResult GetAll() => Ok(_service.ObtenerTodos());

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var paciente = _service.ObtenerPorId(id);
			return paciente == null ? NotFound() : Ok(paciente);
		}
	}
}