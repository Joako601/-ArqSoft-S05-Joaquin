using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración de rutas de datos
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "data");
if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

var csvPacientes = Path.Combine(dataFolder, "pacientes.csv");
var csvMedicos = Path.Combine(dataFolder, "medicos.csv");
var csvCitas = Path.Combine(dataFolder, "citas.csv");

// Registro de servicios (Inyección de Dependencias)
// Estos aseguran que tus Controladores puedan recibir los repositorios
/*
builder.Services.AddScoped<ICitaRepository>(s => new CsvCitaRepository(csvCitas));
builder.Services.AddScoped<IPacienteRepository>(s => new CsvPacienteRepository(csvPacientes));
builder.Services.AddScoped<IMedicoRepository>(s => new CsvMedicoRepository(csvMedicos));
*/

builder.Services.AddScoped<IPacienteRepository, JsonPacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, JsonMedicoRepository>();
builder.Services.AddScoped<ICitaRepository, JsonCitaRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();