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

		private readonly List<ICitaObserver> _observers = new();

		public void AgregarObserver(ICitaObserver observer)
			=> _observers.Add(observer);
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

		public Cita? ObtenerPorId(int id)
		{
			return _repo.ObtenerTodos().FirstOrDefault(c => c.Id == id);
		}

		public Cita? ConfirmarCita(int citaId)
		{
			var cita = ObtenerPorId(citaId);
			if (cita == null) return null;
			cita.Estado = "Confirmada";
			foreach (var observer in _observers)
				observer.OnCitaConfirmada(cita);
			return cita;
		}

	}
}
