using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Citas_App.Infrastructure.db;

namespace Citas_App.Infrastructure.Repositories
{
	public class PostgresMedicoRepository : IMedicoRepository
	{
		private readonly CitasDbContext _context;

		public PostgresMedicoRepository(CitasDbContext context)
		{
			_context = context;
		}

		public List<Medico> ObtenerTodos() =>
			_context.Medicos.OrderBy(m => m.Id).ToList();

		public Medico? ObtenerPorId(int id) =>
			_context.Medicos.FirstOrDefault(m => m.Id == id);

		public void Agregar(Medico medico)
		{
			_context.Medicos.Add(medico);
			_context.SaveChanges();
		}
	}
}