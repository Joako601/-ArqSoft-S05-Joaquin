using Microsoft.AspNetCore.Mvc;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;

namespace Citas_App.Web.Controllers
{
	public class CitaController : Controller
	{
		private readonly ICitaRepository _citaRepo;
		private readonly IPacienteRepository _pacienteRepo;
		private readonly IMedicoRepository _medicoRepo;

		public CitaController(ICitaRepository citaRepo, IPacienteRepository pacienteRepo,
							  IMedicoRepository medicoRepo) { 
		_citaRepo = citaRepo;
            _pacienteRepo = pacienteRepo;
            _medicoRepo = medicoRepo;
        }

		public IActionResult Index()
		{
			ViewBag.Pacientes = _pacienteRepo.ObtenerTodos();
			ViewBag.Medicos = _medicoRepo.ObtenerTodos();
			return View(_citaRepo.ObtenerTodos());
		}

		public IActionResult PorPaciente(int pacienteId)
		{
			ViewBag.Pacientes = _pacienteRepo.ObtenerTodos();
			ViewBag.Medicos = _medicoRepo.ObtenerTodos();
			return View(_citaRepo.ObtenerPorPaciente(pacienteId));
		}

		[HttpGet]
		public IActionResult Crear()
		{
			
			ViewBag.Pacientes = _pacienteRepo.ObtenerTodos();
			ViewBag.Medicos = _medicoRepo.ObtenerTodos();
			return View();
		}

		[HttpPost]
		public IActionResult Crear(Cita nuevoCita)
		{
			if (ModelState.IsValid)
			{
				_citaRepo.Agregar(nuevoCita);
				return RedirectToAction("Index");
			}

			
			ViewBag.Pacientes = _pacienteRepo.ObtenerTodos();
			ViewBag.Medicos = _medicoRepo.ObtenerTodos();
			return View(nuevoCita);
		}
	}
}
