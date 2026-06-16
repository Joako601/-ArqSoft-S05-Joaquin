using Citas_App.Domain.Models;

namespace Citas_App.Domain.Interfaces
{
	public interface ICitaRepository
	{
		List<Cita> ObtenerTodos();
		List<Cita> ObtenerPorPaciente(int pacienteId);

		void Agregar(Cita cita);
	}
}