using System.Text.Json;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Citas_App.Infrastructure.Repositories
{
	public class JsonPacienteRepository : IPacienteRepository
	{
		private readonly string _path;
		private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

		public JsonPacienteRepository(IWebHostEnvironment env)
		{
			_path = Path.Combine(env.ContentRootPath, "data", "pacientes.json");
		}

		public List<Paciente> ObtenerTodos()
		{
			if (!File.Exists(_path)) return new();
			var json = File.ReadAllText(_path);
			return JsonSerializer.Deserialize<List<Paciente>>(json, _options) ?? new();
		}

		public Paciente? ObtenerPorId(int id) =>
			ObtenerTodos().FirstOrDefault(p => p.Id == id);

		public void Agregar(Paciente paciente)
		{

			string rutaArchivo = Path.Combine("data", "pacientes.json");


			var jsonActual = File.ReadAllText(rutaArchivo);
			var pacientes = JsonSerializer.Deserialize<List<Paciente>>(jsonActual) ?? new List<Paciente>();

			
			int nuevoId = pacientes.Any() ? pacientes.Max(p => p.Id) + 1 : 1;
			paciente.Id = nuevoId;

			
			pacientes.Add(paciente);

			
			var opciones = new JsonSerializerOptions { WriteIndented = true };
			var nuevoJson = JsonSerializer.Serialize(pacientes, opciones);

			
			File.WriteAllText(rutaArchivo, nuevoJson);
		}
	}
}