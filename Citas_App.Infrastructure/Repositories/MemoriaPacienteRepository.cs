using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;

public class MemoriaPacienteRepository : IPacienteRepository
{
	private readonly List<Paciente> _pacientes = new();

	public List<Paciente> ObtenerTodos() => _pacientes;

	public Paciente? ObtenerPorId(int id) =>
		_pacientes.FirstOrDefault(p => p.Id == id);

	public void Agregar(Paciente paciente) => _pacientes.Add(paciente);
}