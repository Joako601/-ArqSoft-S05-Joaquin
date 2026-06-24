using System.Reflection;
using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;

namespace CitasApp.Infrastructure.Repositories;

public class LoggingPacienteRepository : IPacienteRepository
{

	// DECORATOR — agrega logging sin modificar el repositorio originalpublic class LoggingPacienteRepository : IPacienteRepository    {

	private readonly IPacienteRepository _inner;


	public LoggingPacienteRepository(IPacienteRepository inner)

	{
		_inner = inner;
	}


	public List<Paciente> ObtenerTodos()

	{

		Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerTodos — inicio");

		var resultado = _inner.ObtenerTodos();

		Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerTodos — {resultado.Count} registros");

		return resultado;

	}


	public Paciente? ObtenerPorId(int id)

	{

		Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerPorId({id}) — inicio");

		var resultado = _inner.ObtenerPorId(id);

		Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ObtenerPorId({id}) — {(resultado != null ? "encontrado" : "no encontrado")}");

		return resultado;

	}
	public void Agregar(Paciente paciente)
	{
		Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Agregar — id: {paciente.Id}");
		_inner.Agregar(paciente);
	}

}