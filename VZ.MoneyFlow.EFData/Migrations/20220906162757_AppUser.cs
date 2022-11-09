using Microsoft.EntityFrameworkCore.Migrations;

namespace VZ.MoneyFlow.EFData.Migrations
{
    public partial class AppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsersAccounts",
                columns: table => new
                {
                    AffiliateAccountId = table.Column<int>(type: "int", nullable: false),
                    AffiliateUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsersAccounts", x => new { x.AffiliateUserId, x.AffiliateAccountId });
                    table.ForeignKey(
                        name: "FK_AppUsersAccounts_Accounts_AffiliateAccountId",
                        column: x => x.AffiliateAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUsersAccounts_AspNetUsers_AffiliateUserId",
                        column: x => x.AffiliateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsersAccounts_AffiliateAccountId",
                table: "AppUsersAccounts",
                column: "AffiliateAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsersAccounts");
        }
    }
}
