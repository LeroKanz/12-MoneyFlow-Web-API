using Microsoft.EntityFrameworkCore.Migrations;

namespace VZ.MoneyFlow.EFData.Migrations
{
    public partial class AddCurrencyIdToTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Transfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CurrencyId",
                table: "Transfers",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Currencies_CurrencyId",
                table: "Transfers",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Currencies_CurrencyId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_CurrencyId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Transfers");
        }
    }
}
