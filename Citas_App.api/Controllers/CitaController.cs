using Citas_.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CitasApp.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CitaController : ControllerBase
	{
		private readonly CitaService _citaService;
		private readonly PacienteService _pacienteService;
		private readonly MedicoService _medicoService;

		public CitaController(CitaService citaService,
							   PacienteService pacienteService,
							   MedicoService medicoService)
		{
			_citaService = citaService;
			_pacienteService = pacienteService;
			_medicoService = medicoService;
		}

		[HttpGet]
		public IActionResult GetAll() => Ok(_citaService.ObtenerTodos());

		[HttpGet("porpaciente/{pacienteId}")]
		public IActionResult PorPaciente(int pacienteId)
		{
			var citas = _citaService.ObtenerPorPaciente(pacienteId);
			return citas.Count == 0 ? NotFound() : Ok(citas);
		}

		[HttpGet("prueba")]
		public IActionResult Prueba()
		{
			return Ok("¡El controlador y las rutas funcionan a la perfección!");
		}
	}
}