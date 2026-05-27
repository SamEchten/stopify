using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stopify.Migrations
{
    /// <inheritdoc />
    public partial class AddSongDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "duration",
                table: "songs",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "songs");
        }
    }
}
