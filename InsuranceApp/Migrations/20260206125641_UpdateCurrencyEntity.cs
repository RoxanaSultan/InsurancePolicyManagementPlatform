using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCurrencyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy");

            migrationBuilder.AddForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy");

            migrationBuilder.AddForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
