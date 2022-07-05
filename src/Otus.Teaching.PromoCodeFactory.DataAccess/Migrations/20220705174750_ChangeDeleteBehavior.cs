using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    public partial class ChangeDeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_Customers_CustomerId",
                table: "CustomerPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_Preferences_PreferenceId",
                table: "CustomerPreference");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "FirstName", "LastName" },
                values: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), "ivan_sergeev@mail.ru", "Иван", "Петров" });

            migrationBuilder.InsertData(
                table: "Preferences",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("ef7f299f-92d7-459f-896e-078ed53ef99c"), "Театр" });

            migrationBuilder.InsertData(
                table: "Preferences",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("c4bda62e-fc74-4256-a956-4760b3858cbd"), "Семья" });

            migrationBuilder.InsertData(
                table: "Preferences",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("76324c47-68d2-472d-abb8-33cfa8cc0c84"), "Дети" });

            migrationBuilder.InsertData(
                table: "CustomerPreference",
                columns: new[] { "CustomerId", "PreferenceId" },
                values: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("ef7f299f-92d7-459f-896e-078ed53ef99c") });

            migrationBuilder.InsertData(
                table: "CustomerPreference",
                columns: new[] { "CustomerId", "PreferenceId" },
                values: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("76324c47-68d2-472d-abb8-33cfa8cc0c84") });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_Customers_CustomerId",
                table: "CustomerPreference",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_Preferences_PreferenceId",
                table: "CustomerPreference",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_Customers_CustomerId",
                table: "CustomerPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_Preferences_PreferenceId",
                table: "CustomerPreference");

            migrationBuilder.DeleteData(
                table: "CustomerPreference",
                keyColumns: new[] { "CustomerId", "PreferenceId" },
                keyValues: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("76324c47-68d2-472d-abb8-33cfa8cc0c84") });

            migrationBuilder.DeleteData(
                table: "CustomerPreference",
                keyColumns: new[] { "CustomerId", "PreferenceId" },
                keyValues: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("ef7f299f-92d7-459f-896e-078ed53ef99c") });

            migrationBuilder.DeleteData(
                table: "Preferences",
                keyColumn: "Id",
                keyValue: new Guid("c4bda62e-fc74-4256-a956-4760b3858cbd"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"));

            migrationBuilder.DeleteData(
                table: "Preferences",
                keyColumn: "Id",
                keyValue: new Guid("76324c47-68d2-472d-abb8-33cfa8cc0c84"));

            migrationBuilder.DeleteData(
                table: "Preferences",
                keyColumn: "Id",
                keyValue: new Guid("ef7f299f-92d7-459f-896e-078ed53ef99c"));

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_Customers_CustomerId",
                table: "CustomerPreference",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_Preferences_PreferenceId",
                table: "CustomerPreference",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
