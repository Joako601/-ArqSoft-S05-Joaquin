using Microsoft.AspNetCore.Mvc;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;

namespace Citas_App.Web.Controllers
{
	public class AgendarController : Controller
	{
		private readonly ICitaRepository _citaRepo;
		private readonly IPacienteRepository _pacienteRepo;
		private readonly IMedicoRepository _medicoRepo;

		// Inyectamos los 3 repositorios que configuraste en Program.cs
		public AgendarController(ICitaRepository citaRepo,
								   IPacienteRepository pacienteRepo,
								   IMedicoRepository medicoRepo)
		{
			_citaRepo = citaRepo;
			_pacienteRepo = pacienteRepo;
			_medicoRepo = medicoRepo;
		}

		public IActionResult Index()
		{
			
			var dashboardData = new AgendarViewModel
			{
				Citas = _citaRepo.ObtenerTodos(),
				Pacientes = _pacienteRepo.ObtenerTodos(),
				Medicos = _medicoRepo.ObtenerTodos()
			};

			return View(dashboardData);
		}
	}
}