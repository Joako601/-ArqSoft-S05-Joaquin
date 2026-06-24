using System.Threading.RateLimiting;
using Citas_.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Observers;
using Citas_App.Infrastructure.Repositories;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Repositorios
builder.Services.AddScoped<IPacienteRepository>(sp =>
{
	var env = sp.GetRequiredService<IWebHostEnvironment>();
	var repo = RepositoryFactory.CrearPacienteRepository(
		builder.Environment.EnvironmentName, env);
	return new LoggingPacienteRepository(repo);
});
builder.Services.AddScoped<IMedicoRepository, JsonMedicoRepository>();
builder.Services.AddScoped<ICitaRepository, JsonCitaRepository>();

builder.Services.AddScoped<CitaService>(sp =>
{
	var repo = sp.GetRequiredService<ICitaRepository>();
	var service = new CitaService(repo);
	service.AgregarObserver(new SmsObserver());
	service.AgregarObserver(new EmailObserver());
	return service;
});

// Servicios de aplicación
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("StrictPolicy", policy =>
	{
		// no olvidarse de cambiar el puerto
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});




var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("StrictPolicy");


app.UseAuthorization();
app.MapControllers();
app.Run();