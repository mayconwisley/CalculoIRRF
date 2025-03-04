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
                Valor = table.Column<double>(type: "REAL", nullable: false)
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
                Valor = table.Column<double>(type: "REAL", nullable: false)
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
                Valor = table.Column<double>(type: "REAL", nullable: false),
                Porcentagem = table.Column<double>(type: "REAL", nullable: false)
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
                BaseCaculo = table.Column<double>(type: "REAL", nullable: false),
                Aliquota = table.Column<double>(type: "REAL", nullable: false)
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
                Valor = table.Column<double>(type: "REAL", nullable: false),
                Porcentagem = table.Column<double>(type: "REAL", nullable: false),
                Deducao = table.Column<double>(type: "REAL", nullable: false)
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
                BaseCaculo = table.Column<double>(type: "REAL", nullable: false),
                Aliquota = table.Column<double>(type: "REAL", nullable: false),
                Deducao = table.Column<double>(type: "REAL", nullable: false),
                Dependente = table.Column<double>(type: "REAL", nullable: false),
                Simplificado = table.Column<double>(type: "REAL", nullable: false)
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
                Valor = table.Column<double>(type: "REAL", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Simplificado", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Dependente",
            columns: new[] { "Id", "Competencia", "Valor" },
            values: new object[] { 1, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 189.59 });

        migrationBuilder.InsertData(
            table: "DescontoMinimo",
            columns: new[] { "Id", "Competencia", "Valor" },
            values: new object[] { 1, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0 });

        migrationBuilder.InsertData(
            table: "Inss",
            columns: new[] { "Id", "Competencia", "Faixa", "Porcentagem", "Valor" },
            values: new object[,]
            {
                { 1, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5, 1045.0 },
                { 2, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9.0, 2089.5999999999999 },
                { 3, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12.0, 3134.4000000000001 },
                { 4, new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14.0, 6101.0600000000004 },
                { 5, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5, 1212.0 },
                { 6, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9.0, 2452.6700000000001 },
                { 7, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12.0, 3679.0 },
                { 8, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14.0, 7087.2200000000003 },
                { 9, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5, 1302.0 },
                { 10, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9.0, 2571.29 },
                { 11, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12.0, 3856.9400000000001 },
                { 12, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14.0, 7507.4899999999998 },
                { 13, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5, 1320.0 },
                { 14, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9.0, 2571.29 },
                { 15, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12.0, 3856.9400000000001 },
                { 16, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14.0, 7507.4899999999998 },
                { 17, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5, 1412.0 },
                { 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9.0, 2666.6799999999998 },
                { 19, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12.0, 4000.0300000000002 },
                { 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14.0, 7786.0200000000004 },
                { 21, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 7.5, 1518.0 },
                { 22, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 9.0, 2793.8800000000001 },
                { 23, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 12.0, 4190.8299999999999 },
                { 24, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 14.0, 8157.4099999999999 }
            });

        migrationBuilder.InsertData(
            table: "Irrf",
            columns: new[] { "Id", "Competencia", "Deducao", "Faixa", "Porcentagem", "Valor" },
            values: new object[,]
            {
                { 1, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, 1, 0.0, 1903.98 },
                { 2, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 142.80000000000001, 2, 7.5, 2826.6500000000001 },
                { 3, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 354.80000000000001, 3, 15.0, 3751.0500000000002 },
                { 4, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 636.13, 4, 22.5, 4664.6800000000003 },
                { 5, new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 869.36000000000001, 5, 27.5, 99999999999.990005 },
                { 6, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, 1, 0.0, 2112.0 },
                { 8, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 158.40000000000001, 2, 7.5, 2826.6500000000001 },
                { 9, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 370.39999999999998, 3, 15.0, 3751.0500000000002 },
                { 10, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 651.73000000000002, 4, 22.5, 4664.6800000000003 },
                { 11, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 884.96000000000004, 5, 27.5, 99999999999.990005 },
                { 12, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, 1, 0.0, 2259.1999999999998 },
                { 13, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 169.44, 2, 7.5, 2826.6500000000001 },
                { 14, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 381.44, 3, 15.0, 3751.0500000000002 },
                { 15, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 662.76999999999998, 4, 22.5, 4664.6800000000003 },
                { 16, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 896.0, 5, 27.5, 99999999999.990005 }
            });

        migrationBuilder.InsertData(
            table: "Simplificado",
            columns: new[] { "Id", "Competencia", "Valor" },
            values: new object[,]
            {
                { 1, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 528.0 },
                { 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 564.79999999999995 }
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
