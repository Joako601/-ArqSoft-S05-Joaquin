using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Citas_App.Infrastructure.db;

namespace Citas_App.Infrastructure.Repositories
{
	public class PostgresPacienteRepository : IPacienteRepository
	{
		private readonly CitasDbContext _context;

		public PostgresPacienteRepository(CitasDbContext context)
		{
			_context = context;
		}

		public List<Paciente> ObtenerTodos() =>
			_context.Pacientes.OrderBy(p => p.Id).ToList();

		public Paciente? ObtenerPorId(int id) =>
			_context.Pacientes.FirstOrDefault(p => p.Id == id);

		public void Agregar(Paciente paciente)
		{
			_context.Pacientes.Add(paciente);
			_context.SaveChanges();
		}
	}
}