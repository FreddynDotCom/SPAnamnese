using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPAnamnese.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class DateTimePaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DATANASCIMENTO",
                table: "tbpaciente",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DATANASCIMENTO",
                table: "tbpaciente",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
