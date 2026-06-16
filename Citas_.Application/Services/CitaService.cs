using System;
using System.Collections.Generic;
using System.Text;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;

namespace Citas_.Application.Services
{
	public class CitaService
	{
		private readonly ICitaRepository _repo;
		//resibe el repo por el constructor
		public CitaService(ICitaRepository repo)
		{
			_repo = repo;
		}
		public List<Cita> ObtenerTodos()
		{
			return _repo.ObtenerTodos();
		}

		public List<Cita> ObtenerPorPaciente(int pacienteId)
		{
			return _repo.ObtenerPorPaciente(pacienteId).ToList();
		}

		public void Agregar(Cita cita)
		{
			_repo.Agregar(cita);
		}

	}
}
