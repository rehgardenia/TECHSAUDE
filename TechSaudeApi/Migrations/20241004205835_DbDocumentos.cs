using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSaude.Server.Migrations
{
    /// <inheritdoc />
    public partial class DbDocumentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exames");

            migrationBuilder.DropTable(
                name: "HistoricoFamiliares");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    DocumentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    DataAssociada = table.Column<DateOnly>(type: "date", nullable: true),
                    DataUpload = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Caminho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TamanhoArquivo = table.Column<long>(type: "bigint", nullable: false),
                    TipoArquivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HistoricoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.DocumentoId);
                    table.ForeignKey(
                        name: "FK_Documentos_HistoricoMedicos_HistoricoId",
                        column: x => x.HistoricoId,
                        principalTable: "HistoricoMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_HistoricoId",
                table: "Documentos",
                column: "HistoricoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.CreateTable(
                name: "Exames",
                columns: table => new
                {
                    ExameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoricoId = table.Column<int>(type: "int", nullable: false),
                    DataExame = table.Column<DateOnly>(type: "date", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultadoExame = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exames", x => x.ExameId);
                    table.ForeignKey(
                        name: "FK_Exames_HistoricoMedicos_HistoricoId",
                        column: x => x.HistoricoId,
                        principalTable: "HistoricoMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoFamiliares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoricoId = table.Column<int>(type: "int", nullable: false),
                    Doenca = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoFamiliares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoFamiliares_HistoricoMedicos_HistoricoId",
                        column: x => x.HistoricoId,
                        principalTable: "HistoricoMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    HistoricoId = table.Column<int>(type: "int", nullable: false),
                    MedicamentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dosagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receita = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.MedicamentoId);
                    table.ForeignKey(
                        name: "FK_Medicamentos_HistoricoMedicos_HistoricoId",
                        column: x => x.HistoricoId,
                        principalTable: "HistoricoMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exames_HistoricoId",
                table: "Exames",
                column: "HistoricoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoFamiliares_HistoricoId",
                table: "HistoricoFamiliares",
                column: "HistoricoId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicamentos_HistoricoId",
                table: "Medicamentos",
                column: "HistoricoId");
        }
    }
}
