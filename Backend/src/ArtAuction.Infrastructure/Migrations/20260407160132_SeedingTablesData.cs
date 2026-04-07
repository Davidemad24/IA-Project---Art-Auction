using ArtAuction.Application.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtAuction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingTablesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert roles
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { 1, Roles.Admin.ToString(), Roles.Admin.ToString().ToUpper(), Guid.NewGuid().ToString() },
                    { 2, Roles.Artist.ToString(), Roles.Artist.ToString().ToUpper(), Guid.NewGuid().ToString() },
                    { 3, Roles.Buyer.ToString(), Roles.Buyer.ToString().ToUpper(), Guid.NewGuid().ToString() }
                }
            );
            
            // Insert categories
            migrationBuilder.InsertData(
                table: "Categories",
                column: "Name",
                values: new object[]
                {
                    "Painting",
                    "Drawing",
                    "Sculpture",
                    "PrintMaking",
                    "Mixed Media"
                }
            );
            
            // Add tags
            migrationBuilder.InsertData(
                table: "Tags",
                column: "Name",
                values: new object[]
                {
                    "Realism",
                    "Abstract",
                    "Impressionism",
                    "Surrealism",
                    "Expressionism",
                    "Oil",
                    "Acrylic",
                    "Watercolor",
                    "Charcoal",
                    "Ink",
                    "Portrait",
                    "Landscape",
                    "Still Life",
                    "Nature",
                    "Human"
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete roles
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Name",
                keyValues: new object[]
                {
                    Roles.Admin.ToString(),
                    Roles.Artist.ToString(),
                    Roles.Buyer.ToString()
                } 
            );
            
            // Delete categories and tags
            migrationBuilder.Sql("DELETE FROM [dbo].[Categories]");
            migrationBuilder.Sql("DELETE FROM [dbo].[Tags]");
        }
    }
}
