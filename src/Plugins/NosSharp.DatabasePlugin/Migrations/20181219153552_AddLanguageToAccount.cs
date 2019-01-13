using Microsoft.EntityFrameworkCore.Migrations;

namespace SaltyEmu.DatabasePlugin.Migrations
{
    public partial class AddLanguageToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "account");

            migrationBuilder.DropColumn(
                name: "RegistrationEmail",
                table: "account");

            migrationBuilder.DropColumn(
                name: "RegistrationIp",
                table: "account");

            migrationBuilder.DropColumn(
                name: "RegistrationToken",
                table: "account");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "account",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "account");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "account",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationEmail",
                table: "account",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationIp",
                table: "account",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationToken",
                table: "account",
                maxLength: 32,
                nullable: true);
        }
    }
}
