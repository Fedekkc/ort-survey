using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrtSurvey.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRespuestasSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Encuesta_Usuarios_Usuarioid_usuario",
                table: "Encuesta");

            migrationBuilder.DropForeignKey(
                name: "FK_Opciones_Preguntas_Preguntaid_pregunta",
                table: "Opciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Preguntas_Encuesta_Encuestaid_encuesta",
                table: "Preguntas");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Preguntas_Preguntaid_pregunta",
                table: "Respuestas");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_RespuestasEncuestas_RespuestaEncuestaid_respuesta_encuesta",
                table: "Respuestas");

            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_RespuestasEncuestas_id_respuesta_encuesta",
                table: "Respuestas");

            migrationBuilder.DropTable(
                name: "RespuestasEncuestas");

            migrationBuilder.DropIndex(
                name: "IX_Respuestas_id_respuesta_encuesta",
                table: "Respuestas");

            migrationBuilder.DropIndex(
                name: "IX_Respuestas_Preguntaid_pregunta",
                table: "Respuestas");

            migrationBuilder.DropIndex(
                name: "IX_Preguntas_Encuestaid_encuesta",
                table: "Preguntas");

            migrationBuilder.DropIndex(
                name: "IX_Opciones_Preguntaid_pregunta",
                table: "Opciones");

            migrationBuilder.DropIndex(
                name: "IX_Encuesta_Usuarioid_usuario",
                table: "Encuesta");

            migrationBuilder.DropColumn(
                name: "Preguntaid_pregunta",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "id_respuesta_encuesta",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "Encuestaid_encuesta",
                table: "Preguntas");

            migrationBuilder.DropColumn(
                name: "Preguntaid_pregunta",
                table: "Opciones");

            migrationBuilder.DropColumn(
                name: "Usuarioid_usuario",
                table: "Encuesta");

            migrationBuilder.DropIndex(
                name: "IX_Respuestas_RespuestaEncuestaid_respuesta_encuesta",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "RespuestaEncuestaid_respuesta_encuesta",
                table: "Respuestas");

            migrationBuilder.AddColumn<int>(
                name: "id_usuario",
                table: "Respuestas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_id_usuario",
                table: "Respuestas",
                column: "id_usuario");

            migrationBuilder.AlterColumn<string>(
                name: "genero",
                table: "Usuarios",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_respuesta",
                table: "Respuestas",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<string>(
                name: "ip_respondedor",
                table: "Respuestas",
                type: "varchar(45)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "submission_id",
                table: "Respuestas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_reporte",
                table: "Reportes",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_pregunta",
                table: "Preguntas",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)");

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "Encuesta",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Usuarios_id_usuario",
                table: "Respuestas",
                column: "id_usuario",
                principalTable: "Usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Respuestas_Usuarios_id_usuario",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "fecha_respuesta",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "ip_respondedor",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "submission_id",
                table: "Respuestas");

            migrationBuilder.DropIndex(
                name: "IX_Respuestas_id_usuario",
                table: "Respuestas");

            migrationBuilder.DropColumn(
                name: "id_usuario",
                table: "Respuestas");

            migrationBuilder.AlterColumn<string>(
                name: "genero",
                table: "Usuarios",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Preguntaid_pregunta",
                table: "Respuestas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_respuesta_encuesta",
                table: "Respuestas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_reporte",
                table: "Reportes",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_pregunta",
                table: "Preguntas",
                type: "varchar(30)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Encuestaid_encuesta",
                table: "Preguntas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Preguntaid_pregunta",
                table: "Opciones",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "Encuesta",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Usuarioid_usuario",
                table: "Encuesta",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RespuestasEncuestas",
                columns: table => new
                {
                    id_respuesta_encuesta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_encuesta = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    fecha_respuesta = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    ip_respondedor = table.Column<string>(type: "varchar(45)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_id_respuesta_encuesta",
                table: "Respuestas",
                column: "id_respuesta_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Respuestas_Preguntaid_pregunta",
                table: "Respuestas",
                column: "Preguntaid_pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Preguntas_Encuestaid_encuesta",
                table: "Preguntas",
                column: "Encuestaid_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Opciones_Preguntaid_pregunta",
                table: "Opciones",
                column: "Preguntaid_pregunta");

            migrationBuilder.CreateIndex(
                name: "IX_Encuesta_Usuarioid_usuario",
                table: "Encuesta",
                column: "Usuarioid_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasEncuestas_id_encuesta",
                table: "RespuestasEncuestas",
                column: "id_encuesta");

            migrationBuilder.CreateIndex(
                name: "IX_RespuestasEncuestas_id_usuario",
                table: "RespuestasEncuestas",
                column: "id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Encuesta_Usuarios_Usuarioid_usuario",
                table: "Encuesta",
                column: "Usuarioid_usuario",
                principalTable: "Usuarios",
                principalColumn: "id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Opciones_Preguntas_Preguntaid_pregunta",
                table: "Opciones",
                column: "Preguntaid_pregunta",
                principalTable: "Preguntas",
                principalColumn: "id_pregunta");

            migrationBuilder.AddForeignKey(
                name: "FK_Preguntas_Encuesta_Encuestaid_encuesta",
                table: "Preguntas",
                column: "Encuestaid_encuesta",
                principalTable: "Encuesta",
                principalColumn: "id_encuesta");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_Preguntas_Preguntaid_pregunta",
                table: "Respuestas",
                column: "Preguntaid_pregunta",
                principalTable: "Preguntas",
                principalColumn: "id_pregunta");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_RespuestasEncuestas_RespuestaEncuestaid_respuesta_encuesta",
                table: "Respuestas",
                column: "RespuestaEncuestaid_respuesta_encuesta",
                principalTable: "RespuestasEncuestas",
                principalColumn: "id_respuesta_encuesta");

            migrationBuilder.AddForeignKey(
                name: "FK_Respuestas_RespuestasEncuestas_id_respuesta_encuesta",
                table: "Respuestas",
                column: "id_respuesta_encuesta",
                principalTable: "RespuestasEncuestas",
                principalColumn: "id_respuesta_encuesta",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
