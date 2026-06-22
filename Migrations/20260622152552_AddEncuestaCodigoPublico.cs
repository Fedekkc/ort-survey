using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrtSurvey.Migrations
{
    /// <inheritdoc />
    public partial class AddEncuestaCodigoPublico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "codigo_publico",
                table: "Encuesta",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Encuesta
                SET codigo_publico = LOWER(REPLACE(CAST(NEWID() AS varchar(36)), '-', ''))
                WHERE codigo_publico IS NULL OR codigo_publico = '';
            ");

            migrationBuilder.AlterColumn<string>(
                name: "codigo_publico",
                table: "Encuesta",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encuesta_codigo_publico",
                table: "Encuesta",
                column: "codigo_publico",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Encuesta_codigo_publico",
                table: "Encuesta");

            migrationBuilder.DropColumn(
                name: "codigo_publico",
                table: "Encuesta");
        }
    }
}
