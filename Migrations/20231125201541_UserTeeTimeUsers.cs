using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeMate_ServerSide.Migrations
{
    public partial class UserTeeTimeUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeeTimeTeeTimeUser");

            migrationBuilder.AddColumn<int>(
                name: "TeeTimeUserId",
                table: "TeeTimes",
                type: "integer",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeeTimes_TeeTimeUsers_TeeTimeUserId",
                table: "TeeTimes");

            migrationBuilder.DropIndex(
                name: "IX_TeeTimes_TeeTimeUserId",
                table: "TeeTimes");

            migrationBuilder.DropColumn(
                name: "TeeTimeUserId",
                table: "TeeTimes");

            migrationBuilder.CreateTable(
                name: "TeeTimeTeeTimeUser",
                columns: table => new
                {
                    TeeTimeUsersId = table.Column<int>(type: "integer", nullable: false),
                    TeeTimesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeeTimeTeeTimeUser", x => new { x.TeeTimeUsersId, x.TeeTimesId });
                    table.ForeignKey(
                        name: "FK_TeeTimeTeeTimeUser_TeeTimes_TeeTimesId",
                        column: x => x.TeeTimesId,
                        principalTable: "TeeTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeeTimeTeeTimeUser_TeeTimeUsers_TeeTimeUsersId",
                        column: x => x.TeeTimeUsersId,
                        principalTable: "TeeTimeUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeeTimeTeeTimeUser_TeeTimesId",
                table: "TeeTimeTeeTimeUser",
                column: "TeeTimesId");
        }
    }
}
