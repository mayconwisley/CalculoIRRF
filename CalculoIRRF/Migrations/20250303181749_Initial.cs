using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalculoIRRF.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Dependente",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                Valor = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Dependente", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DescontoMinimo",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                Valor = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DescontoMinimo", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Inss",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                Faixa = table.Column<int>(type: "INTEGER", nullable: false),
                Valor = table.Column<decimal>(type: "TEXT", nullable: false),
                Porcentagem = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Inss", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "InssGov",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                Sequencia = table.Column<int>(type: "INTEGER", nullable: false),
                BaseCaculo = table.Column<decimal>(type: "TEXT", nullable: false),
                Aliquota = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InssGov", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Irrf",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                Faixa = table.Column<int>(type: "INTEGER", nullable: false),
                Valor = table.Column<decimal>(type: "TEXT", nullable: false),
                Porcentagem = table.Column<decimal>(type: "TEXT", nullable: false),
                Deducao = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Irrf", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "IrrfRfb",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                Sequencia = table.Column<int>(type: "INTEGER", nullable: false),
                BaseCaculo = table.Column<decimal>(type: "TEXT", nullable: false),
                Aliquota = table.Column<decimal>(type: "TEXT", nullable: false),
                Deducao = table.Column<decimal>(type: "TEXT", nullable: false),
                Dependente = table.Column<decimal>(type: "TEXT", nullable: false),
                Simplificado = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IrrfRfb", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Simplificado",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Competencia = table.Column<DateTime>(type: "TEXT", nullable: false),
                Valor = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Simplificado", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Dependente",
            columns: new[] { "Id", "Competencia", "Valor" },
            values: new object[] { 1, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 189.59m });

        migrationBuilder.InsertData(
            table: "DescontoMinimo",
            columns: new[] { "Id", "Competencia", "Valor" },
            values: new object[] { 1, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10m });

        migrationBuilder.InsertData(
            table: "Inss",
            columns: new[] { "Id", "Competencia", "Faixa", "Porcentagem", "Valor" },
            values: new object[,]
            {
                { 1, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5m, 1045.00m },
                { 2, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9m, 2089.60m },
                { 3, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12m, 3134.40m },
                { 4, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14m, 6101.06m },
                { 5, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5m, 1212.00m },
                { 6, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9m, 2452.67m },
                { 7, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12m, 3679.00m },
                { 8, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14m, 7087.22m },
                { 9, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5m, 1302.00m },
                { 10, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9m, 2571.29m },
                { 11, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12m, 3856.94m },
                { 12, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14m, 7507.49m },
                { 13, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5m, 1320.00m },
                { 14, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9m, 2571.29m },
                { 15, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12m, 3856.94m },
                { 16, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14m, 7507.49m },
                { 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5m, 1412.00m },
                { 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9m, 2666.68m },
                { 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12m, 4000.03m },
                { 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14m, 7786.02m },
                { 21, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5m, 1518.00m },
                { 22, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9m, 2793.88m },
                { 23, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12m, 4190.83m },
                { 24, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14m, 8157.41m }
            });

        migrationBuilder.InsertData(
            table: "Irrf",
            columns: new[] { "Id", "Competencia", "Deducao", "Faixa", "Porcentagem", "Valor" },
            values: new object[,]
            {
                { 1, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 1, 0m, 1903.98m },
                { 2, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 142.80m, 2, 7.50m, 2826.65m },
                { 3, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 354.80m, 3, 15m, 3751.05m },
                { 4, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 636.13m, 4, 22.50m, 4664.68m },
                { 5, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 869.36m, 5, 27.50m, 99999999999.99m },
                { 6, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 1, 0m, 2112.00m },
                { 8, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 158.40m, 2, 7.50m, 2826.65m },
                { 9, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 370.40m, 3, 15m, 3751.05m },
                { 10, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 651.73m, 4, 22.50m, 4664.68m },
                { 11, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 884.96m, 5, 27.50m, 99999999999.99m },
                { 12, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 1, 0m, 2259.20m },
                { 13, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 169.44m, 2, 7.50m, 2826.65m },
                { 14, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 381.44m, 3, 15m, 3751.05m },
                { 15, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 662.77m, 4, 22.50m, 4664.68m },
                { 16, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 896.00m, 5, 27.50m, 99999999999.99m }
            });

        migrationBuilder.InsertData(
            table: "Simplificado",
            columns: new[] { "Id", "Competencia", "Valor" },
            values: new object[,]
            {
                { 1, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 528.00m },
                { 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 564.80m }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Dependente");

        migrationBuilder.DropTable(
            name: "DescontoMinimo");

        migrationBuilder.DropTable(
            name: "Inss");

        migrationBuilder.DropTable(
            name: "InssGov");

        migrationBuilder.DropTable(
            name: "Irrf");

        migrationBuilder.DropTable(
            name: "IrrfRfb");

        migrationBuilder.DropTable(
            name: "Simplificado");
    }
}
