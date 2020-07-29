using Microsoft.EntityFrameworkCore.Migrations;

namespace CricketFavourites.Data.Migrations
{
    public partial class addDbSetForApplicationUserFavourite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserFavourite_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserFavourite");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserFavourite_Favourites_FavouriteId",
                table: "ApplicationUserFavourite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserFavourite",
                table: "ApplicationUserFavourite");

            migrationBuilder.RenameTable(
                name: "ApplicationUserFavourite",
                newName: "ApplicationUserFavourites");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserFavourite_FavouriteId",
                table: "ApplicationUserFavourites",
                newName: "IX_ApplicationUserFavourites_FavouriteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserFavourites",
                table: "ApplicationUserFavourites",
                columns: new[] { "ApplicationUserId", "FavouriteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserFavourites_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserFavourites",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserFavourites_Favourites_FavouriteId",
                table: "ApplicationUserFavourites",
                column: "FavouriteId",
                principalTable: "Favourites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserFavourites_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserFavourites");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserFavourites_Favourites_FavouriteId",
                table: "ApplicationUserFavourites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserFavourites",
                table: "ApplicationUserFavourites");

            migrationBuilder.RenameTable(
                name: "ApplicationUserFavourites",
                newName: "ApplicationUserFavourite");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserFavourites_FavouriteId",
                table: "ApplicationUserFavourite",
                newName: "IX_ApplicationUserFavourite_FavouriteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserFavourite",
                table: "ApplicationUserFavourite",
                columns: new[] { "ApplicationUserId", "FavouriteId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserFavourite_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserFavourite",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserFavourite_Favourites_FavouriteId",
                table: "ApplicationUserFavourite",
                column: "FavouriteId",
                principalTable: "Favourites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
