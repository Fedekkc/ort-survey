using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrtSurvey.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    correo = table.Column<string>(type: "varchar(120)", nullable: false),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    genero = table.Column<string>(type: "varchar(20)", nullable: false),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "Encuesta",
                columns: table => new
                {
                    id_encuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    es_publica = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    fecha_cierre = table.Column<DateTime>(type: "datetime", nullable: true),
                    estado = table.Column<string>(type: "varchar(20)", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    Usuarioid_usuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encuesta", x => x.id_encuesta);
                    table.ForeignKey(
                        name: "FK_Encuesta_Usuarios_Usuarioid_usuario",
                        column: x => x.Usuarioid_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario");
                    table.ForeignKey(
                        name: "FK_Encuesta_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    id_pregunta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    texto = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    id_encuesta = table.Column<int>(type: "int", nullable: false),
                    Encuestaid_encuesta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", x => x.id_pregunta);
                    table.ForeignKey(
                        name: "FK_Preguntas_Encuesta_Encuestaid_encuesta",
                        column: x => x.Encuestaid_encuesta,
                        principalTable: "Encuesta",
                        principalColumn: "id_encuesta");
                    table.ForeignKey(
                        name: "FK_Preguntas_Encuesta_id_encuesta",
                        column: x => x.id_encuesta,
                        principalTable: "Encuesta",
                        principalColumn: "id_encuesta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    id_reporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo_reporte = table.Column<string>(type: "varchar(20)", nullable: false),
                    fecha_generacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    correo_destino = table.Column<string>(type: "varchar(120)", nullable: true),
                    id_encuesta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.id_reporte);
                    table.ForeignKey(
                        name: "FK_Reportes_Encuesta_id_encuesta",
                        column: x => x.id_encuesta,
                        principalTable: "Encuesta",
                        principalColumn: "id_encuesta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RespuestasEncuestas",
                columns: table => new
                {
                    id_respuesta_encuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_encuesta = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    ip_respondedor = table.Column<string>(type: "varchar(45)", nullable: false),
                    fecha_respuesta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespuestasEncuestas", x => x.id_respuesta_encuesta);
                    table.ForeignKey(
                        name: "FK_RespuestasEncuestas_Encuesta_id_encuesta",
                        column: x => x.id_encuesta,
                        principalTable: "Encuesta",
                        principalColumn: "id_encuesta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RespuestasEncuestas_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Opciones",
                columns: table => new
                {
                    id_opcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    texto = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    id_pregunta = table.Column<int>(type: "int", nullable: false),
                    Preguntaid_pregunta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opciones", x => x.id_opcion);
                    table.ForeignKey(
                        name: "FK_Opciones_Preguntas_Preguntaid_pregunta",
                        column: x => x.Preguntaid_pregunta,
                        principalTable: "Preguntas",
                        principalColumn: "id_pregunta");
                    table.ForeignKey(
                        name: "FK_Opciones_Preguntas_id_pregunta",
                        column: x => x.id_pregunta,
                        principalTable: "Preguntas",
                        principalColumn: "id_pregunta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    id_respuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_respuesta_encuesta = table.Column<int>(type: "int", nullable: false),
                    id_pregunta = table.Column<int>(type: "int", nullable: false),
                    id_opcion = table.Column<int>(type: "int", nullable: true),
                    valor_texto = table.Column<string>(type: "varchar(500)", nullable: true),
                    Preguntaid_pregunta = table.Column<int>(type: "int", nullable: true),
                    RespuestaEncuestaid_respuesta_encuesta = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.id_respuesta);
                    table.ForeignKey(
                        name: "FK_Respuestas_Opciones_id_opcion",
                        column: x => x.id_opcion,
                        principalTable: "Opciones",
                        principalColumn: "id_opcion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Respuestas_Preguntas_Preguntaid_pregunta",
                        column: x => x.Preguntaid_pregunta,
                        principalTable: "Preguntas",
                        principalColumn: "id_pregunta");
                    table.ForeignKey(
                        name: "FK_Respuestas_Preguntas_id_pregunta",
                        column: x => x.id_pregunta,
                        principalTable: "Preguntas",
                        principalColumn: "id_pregunta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Respuestas_RespuestasEncuestas_RespuestaEncuestaid_respuesta_encuesta",
                        column: x => x.RespuestaEncuestaid_respuesta_encuesta,
                        principalTable: "RespuestasEncuestas",
                        principalColumn: "id_respuesta_encuesta");
                    table.ForeignKey(
                        name: "FK_Respuestas_RespuestasEncuestas_id_respuesta_encuesta",
                        column: x => x.id_respuesta_encuesta,
                        principalTable: "RespuestasEncuestas",
                        principalColumn: "id_respuesta_encuesta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Encuesta_id_usuario",
                table: "Encuesta",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Encuesta_Usuarioid_usuario",
                table: "Encuesta",
                column: "Usuarioid_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_id_pregunta",
                table: "Opciones",
                column: "id_pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_Preguntaid_pregunta",
                table: "Opciones",
                column: "Preguntaid_pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_Encuestaid_encuesta",
                table: "Preguntas",
                column: "Encuestaid_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_id_encuesta",
                table: "Preguntas",
                column: "id_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_id_encuesta",
                table: "Reportes",
                column: "id_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_id_opcion",
                table: "Respuestas",
                column: "id_opcion");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_id_pregunta",
                table: "Respuestas",
                column: "id_pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_id_respuesta_encuesta",
                table: "Respuestas",
                column: "id_respuesta_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_Preguntaid_pregunta",
                table: "Respuestas",
                column: "Preguntaid_pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_RespuestaEncuestaid_respuesta_encuesta",
                table: "Respuestas",
                column: "RespuestaEncuestaid_respuesta_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasEncuestas_id_encuesta",
                table: "RespuestasEncuestas",
                column: "id_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasEncuestas_id_usuario",
                table: "RespuestasEncuestas",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_correo",
                table: "Usuarios",
                column: "correo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "Opciones");

            migrationBuilder.DropTable(
                name: "RespuestasEncuestas");

            migrationBuilder.DropTable(
                name: "Preguntas");

            migrationBuilder.DropTable(
                name: "Encuesta");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
