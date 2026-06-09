using System.Collections.Generic;


namespace Citas_App.Models;

public class AgendarViewModel 
{
	public List<Cita> Citas { get; set; } = new List<Cita>();
	public List<Paciente> Pacientes { get; set; } = new List<Paciente>();
	public List<Medico> Medicos { get; set; } = new List<Medico>();
}