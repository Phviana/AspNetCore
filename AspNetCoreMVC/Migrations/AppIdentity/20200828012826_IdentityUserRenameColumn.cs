using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreMVC.Migrations.AppIdentity
{
    public partial class IdentityUserRenameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Conty",
                table: "AspNetUsers",
                newName: "County");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "County",
                table: "AspNetUsers",
                newName: "Conty");
        }
    }
}
