using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculoIRRF.Migrations
{
    /// <inheritdoc />
    public partial class V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 18,
                column: "Deducao",
                value: 182.16);

            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 19,
                column: "Deducao",
                value: 394.16000000000003);

            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 20,
                column: "Deducao",
                value: 675.49000000000001);

            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 21,
                column: "Deducao",
                value: 908.73000000000002);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 18,
                column: "Deducao",
                value: 169.44);

            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 19,
                column: "Deducao",
                value: 381.44);

            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 20,
                column: "Deducao",
                value: 662.76999999999998);

            migrationBuilder.UpdateData(
                table: "Irrf",
                keyColumn: "Id",
                keyValue: 21,
                column: "Deducao",
                value: 896.0);
        }
    }
}
