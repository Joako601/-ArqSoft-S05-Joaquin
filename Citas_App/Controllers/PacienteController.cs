using Citas_App.Models;
using CitasApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Citas_App.Controllers
{
	public class PacienteController : Controller
	{

		
		private readonly IPacienteRepository _repo;

		
		public PacienteController(IPacienteRepository repo)
		{
			_repo = repo;
		}

		public IActionResult Index() => View(_repo.ObtenerTodos());

		public IActionResult Detalle(int id)
		{
			var paciente = _repo.ObtenerPorId(id);
			return paciente == null ? NotFound() : View(paciente);
		}

		
		[HttpGet]
		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Crear(Paciente nuevoPaciente)
		{
			if (ModelState.IsValid)
			{
				
				_repo.Agregar(nuevoPaciente);
				return RedirectToAction("Index");
			}
			return View(nuevoPaciente);
		}
	}
}