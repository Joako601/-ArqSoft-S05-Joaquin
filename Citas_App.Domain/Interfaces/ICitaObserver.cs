using Citas_App.Domain.Models;

namespace Citas_App.Domain.Interfaces
{
	public interface ICitaObserver
	{
		void OnCitaConfirmada(Cita cita);
	}
}