using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VZ.MoneyFlow.EFData.Migrations
{
    public partial class AddExchangeentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exchanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountFrom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountTo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyFrom = table.Column<int>(type: "int", nullable: false),
                    CurrencyTo = table.Column<int>(type: "int", nullable: false),
                    AccountFromId = table.Column<int>(type: "int", nullable: true),
                    AccountToId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exchanges_Accounts_AccountFromId",
                        column: x => x.AccountFromId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exchanges_Accounts_AccountToId",
                        column: x => x.AccountToId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_AccountFromId",
                table: "Exchanges",
                column: "AccountFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_AccountToId",
                table: "Exchanges",
                column: "AccountToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exchanges");
        }
    }
}
