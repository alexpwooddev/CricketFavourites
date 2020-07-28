using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketFavourites.Data.Migrations
{
    public partial class addPidToFavouriteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pid",
                table: "Favourites",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pid",
                table: "Favourites");
        }
    }
}
