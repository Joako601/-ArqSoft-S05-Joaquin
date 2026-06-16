using Citas_App.Domain.Models;


namespace Citas_App.Domain.Interfaces
{
	public interface IMedicoRepository
	{
		List<Medico> ObtenerTodos();
		Medico? ObtenerPorId(int id);

		void Agregar(Medico medico);
	}
}