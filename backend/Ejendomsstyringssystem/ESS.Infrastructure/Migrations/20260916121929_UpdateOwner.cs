using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar",
                table: "owners",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "display_name",
                table: "owners",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "display_name",
                table: "owners");
        }
    }
}
