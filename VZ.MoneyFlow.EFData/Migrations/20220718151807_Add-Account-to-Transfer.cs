using Microsoft.EntityFrameworkCore.Migrations;

namespace VZ.MoneyFlow.EFData.Migrations
{
    public partial class AddAccounttoTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_AccountsCurrencies_AccountCurrencyFromAccountId_AccountCurrencyFromCurrencyId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_AccountsCurrencies_AccountCurrencyToAccountId_AccountCurrencyToCurrencyId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AccountCurrencyFromAccountId_AccountCurrencyFromCurrencyId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AccountCurrencyToAccountId_AccountCurrencyToCurrencyId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "AccountCurrencyFromAccountId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "AccountCurrencyFromCurrencyId",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "AccountCurrencyToCurrencyId",
                table: "Transfers",
                newName: "AccountToId");

            migrationBuilder.RenameColumn(
                name: "AccountCurrencyToAccountId",
                table: "Transfers",
                newName: "AccountFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountFromId",
                table: "Transfers",
                column: "AccountFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountToId",
                table: "Transfers",
                column: "AccountToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_AccountFromId",
                table: "Transfers",
                column: "AccountFromId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_AccountToId",
                table: "Transfers",
                column: "AccountToId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_AccountFromId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_AccountToId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AccountFromId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_AccountToId",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "AccountToId",
                table: "Transfers",
                newName: "AccountCurrencyToCurrencyId");

            migrationBuilder.RenameColumn(
                name: "AccountFromId",
                table: "Transfers",
                newName: "AccountCurrencyToAccountId");

            migrationBuilder.AddColumn<int>(
                name: "AccountCurrencyFromAccountId",
                table: "Transfers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountCurrencyFromCurrencyId",
                table: "Transfers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountCurrencyFromAccountId_AccountCurrencyFromCurrencyId",
                table: "Transfers",
                columns: new[] { "AccountCurrencyFromAccountId", "AccountCurrencyFromCurrencyId" });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_AccountCurrencyToAccountId_AccountCurrencyToCurrencyId",
                table: "Transfers",
                columns: new[] { "AccountCurrencyToAccountId", "AccountCurrencyToCurrencyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_AccountsCurrencies_AccountCurrencyFromAccountId_AccountCurrencyFromCurrencyId",
                table: "Transfers",
                columns: new[] { "AccountCurrencyFromAccountId", "AccountCurrencyFromCurrencyId" },
                principalTable: "AccountsCurrencies",
                principalColumns: new[] { "AccountId", "CurrencyId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_AccountsCurrencies_AccountCurrencyToAccountId_AccountCurrencyToCurrencyId",
                table: "Transfers",
                columns: new[] { "AccountCurrencyToAccountId", "AccountCurrencyToCurrencyId" },
                principalTable: "AccountsCurrencies",
                principalColumns: new[] { "AccountId", "CurrencyId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
