using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "deleted_by",
                table: "applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_applications_deleted_by",
                table: "applications",
                column: "deleted_by");

            migrationBuilder.AddForeignKey(
                name: "FK_applications_users_deleted_by",
                table: "applications",
                column: "deleted_by",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_applications_users_deleted_by",
                table: "applications");

            migrationBuilder.DropIndex(
                name: "IX_applications_deleted_by",
                table: "applications");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "applications");
        }
    }
}
