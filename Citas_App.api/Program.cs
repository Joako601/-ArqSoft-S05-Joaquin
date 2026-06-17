using System.Threading.RateLimiting;
using Citas_.Application.Services;
using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Repositorios
builder.Services.AddScoped<IPacienteRepository, JsonPacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, JsonMedicoRepository>();
builder.Services.AddScoped<ICitaRepository, JsonCitaRepository>();
// Servicios de aplicación
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<CitaService>();

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