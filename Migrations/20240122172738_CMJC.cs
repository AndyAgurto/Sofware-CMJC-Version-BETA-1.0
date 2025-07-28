using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMJC.Migrations
{
    /// <inheritdoc />
    public partial class CMJC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    IDPaciente = table.Column<int>(name: "ID_Paciente", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "Date", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aseguradora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoCivil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ocupacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoSangre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAgregado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Consultas = table.Column<int>(type: "int", nullable: false),
                    AsignacionHistoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.IDPaciente);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IDUsuario = table.Column<int>(name: "ID_Usuario", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NHistoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IDUsuario);
                });

            migrationBuilder.CreateTable(
                name: "HistoriasClinicas",
                columns: table => new
                {
                    IDHistoriaClinica = table.Column<int>(name: "ID_HistoriaClinica", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntecedentesFamiliares = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntecedentesPatologicos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntecedentesQuirurgicos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alergias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDPaciente = table.Column<int>(name: "ID_Paciente", type: "int", nullable: false),
                    IDUsuario = table.Column<int>(name: "ID_Usuario", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriasClinicas", x => x.IDHistoriaClinica);
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Pacientes_ID_Paciente",
                        column: x => x.IDPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "ID_Paciente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoriasClinicas_Usuarios_ID_Usuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    IDConsulta = table.Column<int>(name: "ID_Consulta", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FC = table.Column<int>(type: "int", nullable: false),
                    FR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temperatura = table.Column<double>(type: "float", nullable: false),
                    PresionArterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Peso = table.Column<double>(type: "float", nullable: false),
                    Talla = table.Column<double>(type: "float", nullable: false),
                    IMC = table.Column<double>(type: "float", nullable: false),
                    DescripcionExamenFisico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SintomasySignos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamenesAuxiliares = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tratamiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interconsulta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAtencion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoAtencion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CRecetas = table.Column<int>(type: "int", nullable: false),
                    HistoriaConsultaDiagnosticosWord = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IDHistoriaClinica = table.Column<int>(name: "ID_HistoriaClinica", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.IDConsulta);
                    table.ForeignKey(
                        name: "FK_Consultas_HistoriasClinicas_ID_HistoriaClinica",
                        column: x => x.IDHistoriaClinica,
                        principalTable: "HistoriasClinicas",
                        principalColumn: "ID_HistoriaClinica",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoriasUsuarios",
                columns: table => new
                {
                    IDHistoriaUsuario = table.Column<int>(name: "ID_HistoriaUsuario", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDHistoriaClinica = table.Column<int>(name: "ID_HistoriaClinica", type: "int", nullable: false),
                    IDUsuario = table.Column<int>(name: "ID_Usuario", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriasUsuarios", x => x.IDHistoriaUsuario);
                    table.ForeignKey(
                        name: "FK_HistoriasUsuarios_HistoriasClinicas_ID_HistoriaClinica",
                        column: x => x.IDHistoriaClinica,
                        principalTable: "HistoriasClinicas",
                        principalColumn: "ID_HistoriaClinica",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoriasUsuarios_Usuarios_ID_Usuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Diagnosticos",
                columns: table => new
                {
                    IDDiagnostico = table.Column<int>(name: "ID_Diagnostico", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDiagnostico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CIE10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionDiagnostico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDConsulta = table.Column<int>(name: "ID_Consulta", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosticos", x => x.IDDiagnostico);
                    table.ForeignKey(
                        name: "FK_Diagnosticos_Consultas_ID_Consulta",
                        column: x => x.IDConsulta,
                        principalTable: "Consultas",
                        principalColumn: "ID_Consulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recetas",
                columns: table => new
                {
                    IDReceta = table.Column<int>(name: "ID_Receta", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDConsulta = table.Column<int>(name: "ID_Consulta", type: "int", nullable: false),
                    Paciente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAtencion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArchivoWord = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoMIME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCaducidad = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recetas", x => x.IDReceta);
                    table.ForeignKey(
                        name: "FK_Recetas_Consultas_ID_Consulta",
                        column: x => x.IDConsulta,
                        principalTable: "Consultas",
                        principalColumn: "ID_Consulta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_ID_HistoriaClinica",
                table: "Consultas",
                column: "ID_HistoriaClinica");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosticos_ID_Consulta",
                table: "Diagnosticos",
                column: "ID_Consulta");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_ID_Paciente",
                table: "HistoriasClinicas",
                column: "ID_Paciente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasClinicas_ID_Usuario",
                table: "HistoriasClinicas",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasUsuarios_ID_HistoriaClinica",
                table: "HistoriasUsuarios",
                column: "ID_HistoriaClinica");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriasUsuarios_ID_Usuario",
                table: "HistoriasUsuarios",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Recetas_ID_Consulta",
                table: "Recetas",
                column: "ID_Consulta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diagnosticos");

            migrationBuilder.DropTable(
                name: "HistoriasUsuarios");

            migrationBuilder.DropTable(
                name: "Recetas");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "HistoriasClinicas");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
