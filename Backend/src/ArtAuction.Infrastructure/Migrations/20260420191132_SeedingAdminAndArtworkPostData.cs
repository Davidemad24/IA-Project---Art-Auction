using ArtAuction.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtAuction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAdminAndArtworkPostData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seeding Admin data
            string passwordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, " ");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "Name", "UserType", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", 
                    "PasswordHash", "EmailConfirmed", "SecurityStamp", "PhoneNumberConfirmed", "TwoFactorEnabled", 
                    "LockoutEnabled", "AccessFailedCount" },
                values: new object[,]
                {
                    // Admins
                    { 19, "System Admin", "Admin","admin@art.com", "ADMIN@ART.COM", "admin@art.com", "ADMIN@ART.COM", 
                        passwordHash, true, Guid.NewGuid().ToString(), false, false, true, 0 }
                });

            // 3. Assign Roles to Users
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { 19, 1 } // Admin 1 is an Admin
                });
            
            // Seed Artwork Posts
            migrationBuilder.InsertData(
                table: "ArtworkPosts",
                columns: new[] { 
                    "Id", "Title", "Description", "InitialPrice", "BuyNewPrice", 
                    "StartDate", "EndDate", "Image", "CategoryId", "ArtistId", "AdminId" 
                },
                values: new object[,]
                {
                    { 
                        1, "Dawn on Cairo", "Mixed media cityscape.", 
                        300m, 1000m, DateTime.UtcNow.AddHours(-6), DateTime.UtcNow.AddHours(10), 
                        new byte[0], 1, 5, 19 // Assuming ArtistId 2 and AdminId 1 exist
                    },
                    { 
                        2, "Copper Geometry", "Abstract geometric study.", 
                        450m, 1200m, DateTime.UtcNow.AddHours(-4), DateTime.UtcNow.AddHours(6), 
                        new byte[0], 2, 5, 19
                    },
                    { 
                        3, "Stillness in Indigo", "Large-format portrait.", 
                        500m, 1500m, DateTime.UtcNow.AddHours(6), DateTime.UtcNow.AddHours(20), 
                        new byte[0], 3, 17, null // AdminId is null for 'Pending' posts
                    },
                    { 
                        4, "Golden Alley Echo", "Oil on canvas.", 
                        380m, 900m, DateTime.UtcNow.AddHours(-3), DateTime.UtcNow.AddHours(8), 
                        new byte[0], 1, 7 , 19
                    },
                    { 
                        5, "Silent Geometry No. 9", "Bold geometric composition.", 
                        620m, 1800m, DateTime.UtcNow.AddHours(-2), DateTime.UtcNow.AddHours(12), 
                        new byte[0], 2, 7, 19
                    },
                    { 
                        6, "Nile Moon Passage", "Atmospheric night-scene.", 
                        320m, 850m, DateTime.UtcNow.AddHours(-0.5), DateTime.UtcNow.AddMinutes(5), 
                        new byte[0], 1, 7, 19
                    }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete admin data
            migrationBuilder.DeleteData(table: "AspNetUserRoles", keyColumns: new[] { "UserId", "RoleId" }, 
                keyValues: new object[] { 19, 1 });
            migrationBuilder.DeleteData(table: "AspNetUsers", keyColumn: "Id", keyValues: new object[] { 1 });
            
            // Delete artwork post data
            migrationBuilder.DeleteData(table: "ArtworkPosts", keyColumn: "Id", 
                keyValues: new object[] { 1, 2, 3, 4, 5, 6 });
        }
    }
}
