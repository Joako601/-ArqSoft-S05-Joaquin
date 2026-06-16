using Microsoft.AspNetCore.Mvc;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;

namespace Citas_App.Web.Controllers
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