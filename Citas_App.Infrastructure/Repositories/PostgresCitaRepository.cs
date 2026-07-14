using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Citas_App.Infrastructure.db;

namespace Citas_App.Infrastructure.Repositories
{
	public class PostgresCitaRepository : ICitaRepository
	{
		private readonly CitasDbContext _context;

		public PostgresCitaRepository(CitasDbContext context)
		{
			_context = context;
		}

		public List<Cita> ObtenerTodos() =>
			_context.Citas.OrderBy(c => c.Id).ToList();

		public List<Cita> ObtenerPorPaciente(int pacienteId) =>
			_context.Citas.Where(c => c.PacienteId == pacienteId).ToList();

		public void Agregar(Cita cita)
		{
			_context.Citas.Add(cita);
			_context.SaveChanges();
		}
	}
}