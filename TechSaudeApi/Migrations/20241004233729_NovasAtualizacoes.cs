using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSaude.Server.Migrations
{
    /// <inheritdoc />
    public partial class NovasAtualizacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAssociada",
                table: "Documentos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Documentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Documentos");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataAssociada",
                table: "Documentos",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
