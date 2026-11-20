using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policy_Broker_BrokerId",
                table: "Policy");

            migrationBuilder.DropForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy");

            migrationBuilder.AddForeignKey(
                name: "FK_Policy_Broker_BrokerId",
                table: "Policy",
                column: "BrokerId",
                principalTable: "Broker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policy_Broker_BrokerId",
                table: "Policy");

            migrationBuilder.DropForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy");

            migrationBuilder.AddForeignKey(
                name: "FK_Policy_Broker_BrokerId",
                table: "Policy",
                column: "BrokerId",
                principalTable: "Broker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Policy_Currency_CurrencyId",
                table: "Policy",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
