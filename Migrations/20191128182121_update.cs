using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductCatalogy.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "Product",
                newName: "LastUpdateDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdateDate",
                table: "Product",
                newName: "LastUpdate");
        }
    }
}
