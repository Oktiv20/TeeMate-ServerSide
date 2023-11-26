using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeeMate_ServerSide.Migrations
{
    public partial class NewTeeTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NumOfPlayers",
                table: "TeeTimes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "TeeTimes",
                keyColumn: "Id",
                keyValue: 1,
                column: "NumOfPlayers",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TeeTimes",
                keyColumn: "Id",
                keyValue: 2,
                column: "NumOfPlayers",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumOfPlayers",
                table: "TeeTimes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "TeeTimes",
                keyColumn: "Id",
                keyValue: 1,
                column: "NumOfPlayers",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TeeTimes",
                keyColumn: "Id",
                keyValue: 2,
                column: "NumOfPlayers",
                value: 2);
        }
    }
}
