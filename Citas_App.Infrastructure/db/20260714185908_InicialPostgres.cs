using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Citas_App.Infrastructure.db
{
    /// <inheritdoc />
    public partial class InicialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "medicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Especialidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumeroLicencia = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pacientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pacientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "citas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PacienteId = table.Column<int>(type: "integer", nullable: false),
                    MedicoId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_citas_medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_citas_pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_citas_MedicoId",
                table: "citas",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_citas_PacienteId",
                table: "citas",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "citas");

            migrationBuilder.DropTable(
                name: "medicos");

            migrationBuilder.DropTable(
                name: "pacientes");
        }
    }
}
