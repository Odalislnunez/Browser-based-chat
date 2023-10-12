using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Browser_based_chat.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserIDToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Rooms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "ID",
                keyValue: 1,
                column: "UserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "ID",
                keyValue: 2,
                column: "UserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "ID",
                keyValue: 3,
                column: "UserId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Rooms");
        }
    }
}
