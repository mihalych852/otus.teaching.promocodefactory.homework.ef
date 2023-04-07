using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddActualDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Actual_Date",
                table: "Roles",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actual_Date",
                table: "Roles");
        }
    }
}
