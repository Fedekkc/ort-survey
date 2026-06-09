using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrtSurvey.Migrations
{
    /// <inheritdoc />
    public partial class RemovePreguntaTipoAndObligatoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "es_obligatoria",
                table: "Preguntas");

            migrationBuilder.DropColumn(
                name: "tipo_pregunta",
                table: "Preguntas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "es_obligatoria",
                table: "Preguntas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "tipo_pregunta",
                table: "Preguntas",
                type: "varchar(30)",
                nullable: true);
        }
    }
}
