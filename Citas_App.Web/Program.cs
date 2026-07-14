using Citas_App.Domain.Interfaces;
using Citas_App.Infrastructure.db;
using Citas_App.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using CitasApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configuración de rutas de datos (se mantiene por si otros repos JSON siguen en uso)
var dataFolder = Path.Combine(builder.Environment.ContentRootPath, "data");
if (!Directory.Exists(dataFolder)) Directory.CreateDirectory(dataFolder);

// PostgreSQL
builder.Services.AddDbContext<CitasDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddScoped<IPacienteRepository>(sp =>
{
	var context = sp.GetRequiredService<CitasDbContext>();
	var repo = new PostgresPacienteRepository(context);
	return new LoggingPacienteRepository(repo);
});

builder.Services.AddScoped<IMedicoRepository, PostgresMedicoRepository>();
builder.Services.AddScoped<ICitaRepository, PostgresCitaRepository>();

var app = builder.Build();

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