using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace practice_api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserImgToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserImg",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImg",
                table: "AspNetUsers");
        }
    }
}
