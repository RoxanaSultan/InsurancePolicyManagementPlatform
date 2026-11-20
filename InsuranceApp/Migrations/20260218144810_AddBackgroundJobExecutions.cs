using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBackgroundJobExecutions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolicyCancellation_Policy_PolicyId",
                table: "PolicyCancellation");

            migrationBuilder.DropTable(
                name: "BuildingRiskIndicators");

            migrationBuilder.DropTable(
                name: "RiskIndicators");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "BackgroundJobExecutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastRunDateUtc = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundJobExecutions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundJobExecutions_JobName",
                table: "BackgroundJobExecutions",
                column: "JobName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundJobExecutions");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RiskIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskIndicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingRiskIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    RiskIndicatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingRiskIndicators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingRiskIndicators_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuildingRiskIndicators_RiskIndicators_RiskIndicatorId",
                        column: x => x.RiskIndicatorId,
                        principalTable: "RiskIndicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingRiskIndicators_BuildingId_RiskIndicatorId",
                table: "BuildingRiskIndicators",
                columns: new[] { "BuildingId", "RiskIndicatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingRiskIndicators_RiskIndicatorId",
                table: "BuildingRiskIndicators",
                column: "RiskIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskIndicators_Name",
                table: "RiskIndicators",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyCancellation_Policy_PolicyId",
                table: "PolicyCancellation",
                column: "PolicyId",
                principalTable: "Policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
