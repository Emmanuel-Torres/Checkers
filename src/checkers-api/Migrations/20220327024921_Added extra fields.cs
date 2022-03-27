using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace checkers_api.Migrations
{
    public partial class Addedextrafields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "UserProfiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BestJoke",
                table: "UserProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IceCreamFlavor",
                table: "UserProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pizza",
                table: "UserProfiles",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "BestJoke",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "IceCreamFlavor",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Pizza",
                table: "UserProfiles");
        }
    }
}
