using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtAuction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateArtistApprovalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovedByAdminId",
                table: "Artists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Artists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedByAdminId",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Artists");
        }
    }
}
