using InsuranceApp.Application.Common.ValidatorMessages;
using InsuranceApp.Application.Validators.Report;
using InsuranceApp.Application.Tests.TestData;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.Validators;

public class ReportValidatorsTests
{
    private static ReportFilterRequestValidator CreateValidator() => new();

    [Fact]
    public void ReportFilterRequestValidator_WhenFromDefault_ShouldFail()
    {
        // Arrange
        var validator = CreateValidator();
        var model = ReportValidatorTestData.ValidRequest(from: default(DateTime));

        // Act
        var result = validator.Validate(model);

        // Assert
        AssertInvalid(result, "From", ReportValidatorMessages.FromRequired);
    }

    [Fact]
    public void ReportFilterRequestValidator_WhenToDefault_ShouldFail()
    {
        // Arrange
        var validator = CreateValidator();
        var model = ReportValidatorTestData.ValidRequest(to: default(DateTime));

        // Act
        var result = validator.Validate(model);

        // Assert
        AssertInvalid(result, "To", ReportValidatorMessages.ToRequired);
    }

    [Fact]
    public void ReportFilterRequestValidator_WhenToBeforeFrom_ShouldFail()
    {
        // Arrange
        var validator = CreateValidator();
        var model = ReportValidatorTestData.ValidRequest(
            from: new DateTime(2025, 1, 2),
            to: new DateTime(2025, 1, 1));

        // Act
        var result = validator.Validate(model);

        // Assert
        AssertInvalid(result, string.Empty, ReportValidatorMessages.ToAfterFrom);
    }

    [Fact]
    public void ReportFilterRequestValidator_WhenStatusInvalid_ShouldFail()
    {
        // Arrange
        var validator = CreateValidator();
        var model = ReportValidatorTestData.ValidRequest(status: (PolicyStatusFilter)999);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Status");
    }

    [Fact]
    public void ReportFilterRequestValidator_WhenBuildingTypeInvalid_ShouldFail()
    {
        // Arrange
        var validator = CreateValidator();
        var model = ReportValidatorTestData.ValidRequest(buildingType: (BuildingType)999);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BuildingType");
    }

    [Fact]
    public void ReportFilterRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = CreateValidator();
        var model = ReportValidatorTestData.ValidRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    private static void AssertInvalid(
        FluentValidation.Results.ValidationResult result,
        string propertyName,
        string errorMessage)
    {
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e =>
            (string.IsNullOrEmpty(propertyName) || e.PropertyName == propertyName) &&
            e.ErrorMessage == errorMessage);
    }
}
