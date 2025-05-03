using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalculoIRRF.Migrations
{
    /// <inheritdoc />
    public partial class NovaTabelaIRRF2025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Irrf",
                columns: new[] { "Id", "Competencia", "Deducao", "Faixa", "Porcentagem", "Valor" },
                values: new object[,]
                {
                    { 17, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.0, 1, 0.0, 2428.8000000000002 },
                    { 18, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 169.44, 2, 7.5, 2826.6500000000001 },
                    { 19, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 381.44, 3, 15.0, 3751.0500000000002 },
                    { 20, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 662.76999999999998, 4, 22.5, 4664.6800000000003 },
                    { 21, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 896.0, 5, 27.5, 99999999999.990005 }
                });

            migrationBuilder.InsertData(
                table: "Simplificado",
                columns: new[] { "Id", "Competencia", "Valor" },
                values: new object[] { 3, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 607.20000000000005 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Simplificado",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
