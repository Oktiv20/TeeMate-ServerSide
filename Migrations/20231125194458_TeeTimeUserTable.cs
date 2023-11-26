using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TeeMate_ServerSide.Migrations
{
    public partial class TeeTimeUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeeTimeUserId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeeTimeUserId",
                table: "TeeTimes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeeTimeUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    TeeTimeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeeTimeUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeeTimeUserId",
                table: "Users",
                column: "TeeTimeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeeTimes_TeeTimeUserId",
                table: "TeeTimes",
                column: "TeeTimeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeeTimes_TeeTimeUsers_TeeTimeUserId",
                table: "TeeTimes",
                column: "TeeTimeUserId",
                principalTable: "TeeTimeUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TeeTimeUsers_TeeTimeUserId",
                table: "Users",
                column: "TeeTimeUserId",
                principalTable: "TeeTimeUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeeTimes_TeeTimeUsers_TeeTimeUserId",
                table: "TeeTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_TeeTimeUsers_TeeTimeUserId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TeeTimeUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_TeeTimeUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TeeTimes_TeeTimeUserId",
                table: "TeeTimes");

            migrationBuilder.DropColumn(
                name: "TeeTimeUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TeeTimeUserId",
                table: "TeeTimes");
        }
    }
}
