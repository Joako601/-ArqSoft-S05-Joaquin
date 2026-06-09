using System.Text.Json;
using Citas_App.Models;
using CitasApp.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace CitasApp.Repositories
{
	public class JsonMedicoRepository : IMedicoRepository
	{
		private readonly string _path;
		private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

		public JsonMedicoRepository(IWebHostEnvironment env)
		{
			_path = Path.Combine(env.ContentRootPath, "data", "medicos.json");
		}

		public List<Medico> ObtenerTodos()
		{
			if (!File.Exists(_path)) return new();
			var json = File.ReadAllText(_path);
			return JsonSerializer.Deserialize<List<Medico>>(json, _options) ?? new();
		}

		public Medico? ObtenerPorId(int id) =>
			ObtenerTodos().FirstOrDefault(m => m.Id == id);

		public void Agregar(Medico medico)
		{

			string rutaArchivo = Path.Combine("data", "medicos.json");


			var jsonActual = File.ReadAllText(rutaArchivo);
			var medicos = JsonSerializer.Deserialize<List<Medico>>(jsonActual) ?? new List<Medico>();


			int nuevoId = medicos.Any() ? medicos.Max(p => p.Id) + 1 : 1;
			medico.Id = nuevoId;


			medicos.Add(medico);


			var opciones = new JsonSerializerOptions { WriteIndented = true };
			var nuevoJson = JsonSerializer.Serialize(medicos, opciones);


			File.WriteAllText(rutaArchivo, nuevoJson);
		}
	}
}