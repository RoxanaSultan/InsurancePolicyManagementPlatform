using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRiskFactorConfigurationTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE TRIGGER TR_RiskFactorConfiguration_ValidateReference
ON dbo.RiskFactorConfiguration
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Country
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.Level = 1
          AND NOT EXISTS (SELECT 1 FROM dbo.Countries c WHERE c.Id = i.ReferenceId)
    )
    BEGIN
        RAISERROR('Invalid ReferenceId for Level = Country', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- County
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.Level = 2
          AND NOT EXISTS (SELECT 1 FROM dbo.Counties co WHERE co.Id = i.ReferenceId)
    )
    BEGIN
        RAISERROR('Invalid ReferenceId for Level = County', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- City
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.Level = 3
          AND NOT EXISTS (SELECT 1 FROM dbo.Cities ci WHERE ci.Id = i.ReferenceId)
    )
    BEGIN
        RAISERROR('Invalid ReferenceId for Level = City', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.TR_RiskFactorConfiguration_ValidateReference', 'TR') IS NOT NULL
    DROP TRIGGER dbo.TR_RiskFactorConfiguration_ValidateReference;
");
        }
    }
}
