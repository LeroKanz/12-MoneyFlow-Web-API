using Microsoft.EntityFrameworkCore.Migrations;

namespace VZ.MoneyFlow.EFData.Migrations
{
    public partial class AddAccountIdToExchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyTo",
                table: "Exchanges",
                newName: "CurrencyToId");

            migrationBuilder.RenameColumn(
                name: "CurrencyFrom",
                table: "Exchanges",
                newName: "CurrencyFromId");

            migrationBuilder.AlterColumn<int>(
                name: "AccountToId",
                table: "Exchanges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountFromId",
                table: "Exchanges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyToId",
                table: "Exchanges",
                newName: "CurrencyTo");

            migrationBuilder.RenameColumn(
                name: "CurrencyFromId",
                table: "Exchanges",
                newName: "CurrencyFrom");

            migrationBuilder.AlterColumn<int>(
                name: "AccountToId",
                table: "Exchanges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccountFromId",
                table: "Exchanges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
