using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCoreMVC.Migrations.AppIdentity
{
    public partial class IdentityUserCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Compliment",
                table: "AspNetUsers",
                newName: "Complement");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Complement",
                table: "AspNetUsers",
                newName: "Compliment");
        }
    }
}
