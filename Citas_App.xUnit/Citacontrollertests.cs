using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;
using Citas_App.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Citas_App.xUnit.Controllers
{
	public class CitaRepositoryFake : ICitaRepository
	{
		private readonly List<Cita> _citas;

		public CitaRepositoryFake(List<Cita> citas) => _citas = citas;

		public List<Cita> ObtenerTodos() => _citas;

		public List<Cita> ObtenerPorPaciente(int pacienteId)
			=> _citas.Where(c => c.PacienteId == pacienteId).ToList();

		public void Agregar(Cita cita) => _citas.Add(cita);
	}

	public class PacienteRepositoryFake : IPacienteRepository
	{
		private readonly List<Paciente> _pacientes;

		public PacienteRepositoryFake(List<Paciente> pacientes) => _pacientes = pacientes;

		public List<Paciente> ObtenerTodos() => _pacientes;

		public Paciente? ObtenerPorId(int id) => _pacientes.FirstOrDefault(p => p.Id == id);

		public void Agregar(Paciente paciente) => _pacientes.Add(paciente);
	}

	public class MedicoRepositoryFake : IMedicoRepository
	{
		private readonly List<Medico> _medicos;

		public MedicoRepositoryFake(List<Medico> medicos) => _medicos = medicos;

		public List<Medico> ObtenerTodos() => _medicos;

		public Medico? ObtenerPorId(int id) => _medicos.FirstOrDefault(m => m.Id == id);

		public void Agregar(Medico medico) => _medicos.Add(medico);
	}

	// --------------------------------------------------------------------
	// Pruebas sobre CitaController (Citas_App.Web)
	// --------------------------------------------------------------------
	public class CitaControllerTests
	{
		private CitaController CrearControllerConDatosDePrueba(out List<Cita> citasEsperadas)
		{
			// Arrange — datos de prueba en memoria
			citasEsperadas = new List<Cita>
			{
				new Cita { Id = 1, PacienteId = 10, Estado = "Pendiente" },
				new Cita { Id = 2, PacienteId = 20, Estado = "Confirmada" },
				new Cita { Id = 3, PacienteId = 10, Estado = "Pendiente" }
			};

			var pacientes = new List<Paciente>
			{
				new Paciente { Id = 10, Email = "paciente1@correo.com" },
				new Paciente { Id = 20, Email = "paciente2@correo.com" }
			};

			var medicos = new List<Medico>
			{
				new Medico { Id = 1, Nombre = "Dr. Pérez" }
			};

			// El controller real recibe los repositorios directamente
			var citaRepo = new CitaRepositoryFake(citasEsperadas);
			var pacienteRepo = new PacienteRepositoryFake(pacientes);
			var medicoRepo = new MedicoRepositoryFake(medicos);

			return new CitaController(citaRepo, pacienteRepo, medicoRepo);
		}

		[Fact]
		public void Index_RegresaTodasLasCitasSinFiltrar()
		{
			// Arrange
			var controller = CrearControllerConDatosDePrueba(out var citasEsperadas);

			// Act
			var resultado = controller.Index() as ViewResult;
			var modelo = resultado?.Model as List<Cita>;

			// Assert
			Assert.NotNull(modelo);
			Assert.Equal(citasEsperadas.Count, modelo.Count);
			Assert.Equal(citasEsperadas, modelo);
		}

		[Fact]
		public void Index_IncluyeCitasDeMasDeUnPaciente()
		{
			// Arrange
			var controller = CrearControllerConDatosDePrueba(out _);

			// Act
			var resultado = controller.Index() as ViewResult;
			var modelo = resultado?.Model as List<Cita>;

			// Assert — deben verse citas de distintos pacientes, no solo uno
			Assert.NotNull(modelo);
			var pacientesDistintos = modelo.Select(c => c.PacienteId).Distinct().Count();
			Assert.True(pacientesDistintos > 1);
		}

		[Fact]
		public void Index_CargaCatalogosDePacientesYMedicosEnViewBag()
		{
			// Arrange
			var controller = CrearControllerConDatosDePrueba(out _);

			// Act
			controller.Index();

			// Assert
			Assert.NotNull(controller.ViewBag.Pacientes);
			Assert.NotNull(controller.ViewBag.Medicos);
		}

		[Fact]
		public void PorPaciente_FiltraCitasDelPacienteIndicado()
		{
			// Arrange
			var controller = CrearControllerConDatosDePrueba(out _);

			// Act
			var resultado = controller.PorPaciente(10) as ViewResult;
			var modelo = resultado?.Model as List<Cita>;

			// Assert
			Assert.NotNull(modelo);
			Assert.All(modelo, c => Assert.Equal(10, c.PacienteId));
		}
	}
}