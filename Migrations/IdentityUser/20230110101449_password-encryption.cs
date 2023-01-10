using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebService.API.Migrations.IdentityUser
{
    /// <inheritdoc />
    public partial class passwordencryption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                schema: "Identity",
                table: "users");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                schema: "Identity",
                table: "users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                schema: "Identity",
                table: "users",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "Identity",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                schema: "Identity",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "Identity",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
