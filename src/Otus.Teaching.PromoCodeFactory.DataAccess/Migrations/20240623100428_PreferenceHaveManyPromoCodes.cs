using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PreferenceHaveManyPromoCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Preferences_PreferenceId",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_PreferenceId",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PromoCodeId",
                table: "Preferences");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_PreferenceId",
                table: "PromoCodes",
                column: "PreferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Preferences_PreferenceId",
                table: "PromoCodes",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Preferences_PreferenceId",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_PreferenceId",
                table: "PromoCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "PromoCodeId",
                table: "Preferences",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_PreferenceId",
                table: "PromoCodes",
                column: "PreferenceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Preferences_PreferenceId",
                table: "PromoCodes",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
