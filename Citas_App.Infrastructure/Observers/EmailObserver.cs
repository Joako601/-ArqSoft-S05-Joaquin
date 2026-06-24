using Citas_App.Domain.Interfaces;
using Citas_App.Domain.Models;

namespace Citas_App.Infrastructure.Observers
{
	public class EmailObserver : ICitaObserver
	{
		public void OnCitaConfirmada(Cita cita)
		{
			Console.WriteLine($"[Email] Confirmación enviada al paciente {cita.PacienteId} - motivo: {cita.Motivo} - estado: {cita.Estado}");
		}
	}
}