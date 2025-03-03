using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace CalculoIRRF.Migrations;

/// <inheritdoc />
public partial class Inicial : Migration
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
                Porcentagem = table.Column<decimal>(type: "TEXT", nullable: false),
                Teto = table.Column<decimal>(type: "TEXT", nullable: false)
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
