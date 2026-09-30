using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balise.Migrations
{
    /// <inheritdoc />
    public partial class AddGDPRConsent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNewsletterSubscribed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WantsSatisfactionSurvey",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNewsletterSubscribed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WantsSatisfactionSurvey",
                table: "Users");
        }
    }
}
