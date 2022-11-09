using Microsoft.EntityFrameworkCore.Migrations;

namespace VZ.MoneyFlow.EFData.Migrations
{
    public partial class AddCurrencyIdToOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_CurrencyId",
                table: "Operations",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Currencies_CurrencyId",
                table: "Operations",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Currencies_CurrencyId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_CurrencyId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Operations");
        }
    }
}
