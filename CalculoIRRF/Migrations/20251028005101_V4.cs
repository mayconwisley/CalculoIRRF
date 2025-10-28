using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace CalculoIRRF.Migrations;

/// <inheritdoc />
public partial class V4 : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "DataAtualizacao",
			table: "InssGov");

		migrationBuilder.RenameColumn(
			name: "DataCriacao",
			table: "InssGov",
			newName: "Vigencia");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "Vigencia",
			table: "InssGov",
			newName: "DataCriacao");

		migrationBuilder.AddColumn<DateTime>(
			name: "DataAtualizacao",
			table: "InssGov",
			type: "TEXT",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
	}
}
