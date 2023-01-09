using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebService.API.Migrations.IdentityUser
{
    /// <inheritdoc />
    public partial class ModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Identity",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Identity",
                table: "users",
                type: "datetime2",
                nullable: true);
        }
    }
}
