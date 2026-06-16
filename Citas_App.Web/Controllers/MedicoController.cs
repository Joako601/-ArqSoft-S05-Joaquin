using Microsoft.AspNetCore.Mvc;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;

namespace Citas_App.Web.Controllers
{
	public class MedicoController : Controller
	{
		private readonly IMedicoRepository _repo;
		public MedicoController(IMedicoRepository repo) { _repo = repo; }

		public IActionResult Index() => View(_repo.ObtenerTodos());

		public IActionResult Detalle(int id)
		{
			var medico = _repo.ObtenerPorId(id);
			return medico == null ? NotFound() : View(medico);
		}

		[HttpGet]
		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Crear(Medico nuevoMedico)
		{
			if (ModelState.IsValid)
			{

				_repo.Agregar(nuevoMedico);
				return RedirectToAction("Index");
			}
			return View(nuevoMedico);
		}
	}
}