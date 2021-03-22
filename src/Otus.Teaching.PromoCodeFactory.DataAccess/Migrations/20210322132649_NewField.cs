using Microsoft.EntityFrameworkCore.Migrations;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Migrations
{
    public partial class NewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewField",
                table: "PromoCodes",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewField",
                table: "PromoCodes");
        }
    }
}
