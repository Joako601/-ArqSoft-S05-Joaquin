using System.Reflection.Emit;
using Citas_App.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Citas_App.Infrastructure.db
{
	public class CitasDbContext : DbContext
	{
		public CitasDbContext(DbContextOptions<CitasDbContext> options) : base(options) { }

		public DbSet<Paciente> Pacientes => Set<Paciente>();
		public DbSet<Medico> Medicos => Set<Medico>();
		public DbSet<Cita> Citas => Set<Cita>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Paciente>(e =>
			{
				e.ToTable("pacientes");
				e.HasKey(p => p.Id);
				e.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
				e.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
				e.Property(p => p.Email).HasMaxLength(150);
				e.Property(p => p.Telefono).HasMaxLength(30);
			});

			modelBuilder.Entity<Medico>(e =>
			{
				e.ToTable("medicos");
				e.HasKey(m => m.Id);
				e.Property(m => m.Nombre).IsRequired().HasMaxLength(100);
				e.Property(m => m.Apellido).IsRequired().HasMaxLength(100);
				e.Property(m => m.Especialidad).HasMaxLength(100);
				e.Property(m => m.NumeroLicencia).HasMaxLength(50);
			});

			modelBuilder.Entity<Cita>(e =>
			{
				e.ToTable("citas");
				e.HasKey(c => c.Id);
				e.Property(c => c.Motivo).HasMaxLength(300);
				e.Property(c => c.Estado).HasMaxLength(30);

				
				e.HasOne<Paciente>()
					.WithMany()
					.HasForeignKey(c => c.PacienteId)
					.OnDelete(DeleteBehavior.Restrict);

				e.HasOne<Medico>()
					.WithMany()
					.HasForeignKey(c => c.MedicoId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
		}
}
