using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Migrations
{
    public partial class FakeDataInitializationAndUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var role in FakeDataFactory.Roles)
            {
                migrationBuilder.InsertData(
                    table: "Role",
                    columns: new[] {"Id", "Name", "Description"},
                    values: new object[] {role.Id, role.Name, role.Description}
                );
            }

            foreach (var preference in FakeDataFactory.Preferences)
            {
                migrationBuilder.InsertData(
                    table: "Preference",
                    columns: new[] {"Id", "Name"},
                    values: new object[] {preference.Id, preference.Name}
                );
            }

            foreach (var employee in FakeDataFactory.Employees)
            {
                migrationBuilder.InsertData(
                    table: "Employee",
                    columns: new[] {"Id", "FirstName", "LastName", "Email", "RoleId", "AppliedPromocodesCount"},
                    values: new object[] {employee.Id, employee.FirstName, employee.LastName, employee.Email, employee.Role.Id, employee.AppliedPromocodesCount}
                );
            }
            
            foreach (var customer in FakeDataFactory.Customers)
            {
                migrationBuilder.InsertData(
                    table: "Customer",
                    columns: new[] {"Id", "FirstName", "LastName", "Email"},
                    values: new object[] {customer.Id, customer.FirstName, customer.LastName, customer.Email}
                );

                foreach (var customerPreference in customer.Preferences ?? new List<Preference>())
                {
                    migrationBuilder.InsertData(
                        table: "CustomerPreference",
                        columns: new[] {"CustomersId", "PreferencesId"},
                        values: new object[] {customer.Id, customerPreference.Id}
                    );
                }
            }
            
            //--> Пункт 8 ДЗ
            migrationBuilder.AlterColumn<Guid>(
                table: "Employee",
                name: "RoleId",
                nullable: false
            );

            migrationBuilder.CreateIndex(
                name: "IX_Preference_unique_name",
                table: "Preference",
                column: "Name",
                unique: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Preference_unique_name", table: "Preference");
            
            migrationBuilder.AlterColumn<Guid>(
                table: "Employee",
                name: "RoleId",
                nullable: true
            );
            
            migrationBuilder.Sql("DELETE FROM CustomerPreference", true);
            migrationBuilder.Sql("DELETE FROM Customer", true);
            migrationBuilder.Sql("DELETE FROM Employee", true);
            migrationBuilder.Sql("DELETE FROM Preference", true);
            migrationBuilder.Sql("DELETE FROM Role", true);
        }
    }
}
