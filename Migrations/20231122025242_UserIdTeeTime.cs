using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeMate_ServerSide.Migrations
{
    public partial class UserIdTeeTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TeeTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TeeTimes");
        }
    }
}
