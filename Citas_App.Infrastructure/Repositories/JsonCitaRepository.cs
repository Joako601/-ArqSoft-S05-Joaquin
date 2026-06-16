using System.Text.Json;
using Citas_App.Domain.Models;
using Citas_App.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Citas_App.Infrastructure.Repositories
{
	public class JsonCitaRepository : ICitaRepository
	{
		private readonly string _path;
		private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

		public JsonCitaRepository(IWebHostEnvironment env)
		{
			_path = Path.Combine(env.ContentRootPath, "data", "citas.json");
		}

		public List<Cita> ObtenerTodos()
		{
			if (!File.Exists(_path)) return new();
			var json = File.ReadAllText(_path);
			var citasJson = JsonSerializer.Deserialize<List<CitaJson>>(json, _options) ?? new();
			return citasJson.Select(c => new Cita
			{
				Id = c.Id,
				PacienteId = c.PacienteId,
				MedicoId = c.MedicoId,
				Fecha = DateOnly.Parse(c.Fecha),
				Hora = TimeOnly.Parse(c.Hora),
				Motivo = c.Motivo,
				Estado = c.Estado
			}).ToList();
		}

		public List<Cita> ObtenerPorPaciente(int pacienteId) =>
			ObtenerTodos().Where(c => c.PacienteId == pacienteId).ToList();

		public void Agregar(Cita cita)
		{

			string rutaArchivo = Path.Combine("data", "citas.json");


			var jsonActual = File.ReadAllText(rutaArchivo);
			var citas = JsonSerializer.Deserialize<List<Cita>>(jsonActual) ?? new List<Cita>();


			int nuevoId = citas.Any() ? citas.Max(p => p.Id) + 1 : 1;
			cita.Id = nuevoId;


			citas.Add(cita);


			var opciones = new JsonSerializerOptions { WriteIndented = true };
			var nuevoJson = JsonSerializer.Serialize(citas, opciones);


			File.WriteAllText(rutaArchivo, nuevoJson);
		}
	}
}