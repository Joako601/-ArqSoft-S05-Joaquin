using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Citas_.Application.Services
{
	public class PacienteService
	{

		private readonly IPacienteRepository _repo;

		public PacienteService(IPacienteRepository repo)
		{
			_repo = repo;
		}
		public List<Paciente> ObtenerTodos()
		{
			return _repo.ObtenerTodos();
		}
		public Paciente? ObtenerPorId(int id)
		{
			return _repo.ObtenerPorId(id);
		}
		public void Agregar(Paciente paciente)
		{
			_repo.Agregar(paciente);
		}
	}
}
