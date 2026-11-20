using InsuranceApp.Application.Validators.Policy;
using InsuranceApp.Application.Tests.TestData;

namespace InsuranceApp.Application.Tests.Validators;

public class PolicyValidatorsTests
{
    // PolicyBaseValidator

    [Fact]
    public void PolicyBaseValidator_WhenStartDateDefault_ShouldFail()
    {
        // Arrange
        var validator = new PolicyBaseValidator();
        var model = PolicyValidatorTestData.ValidPolicyBase(startDate: new DateTime());

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "StartDate" &&
                                           e.ErrorMessage == "StartDate is required.");
    }

    [Fact]
    public void PolicyBaseValidator_WhenEndDateDefault_ShouldFail()
    {
        // Arrange
        var validator = new PolicyBaseValidator();
        var model = PolicyValidatorTestData.ValidPolicyBase(endDate: new DateTime());

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "EndDate" &&
                                           e.ErrorMessage == "EndDate is required.");
    }

    [Fact]
    public void PolicyBaseValidator_WhenEndDateBeforeStartDate_ShouldFail()
    {
        // Arrange
        var validator = new PolicyBaseValidator();
        var model = PolicyValidatorTestData.ValidPolicyBase(
            startDate: new DateTime(2025, 1, 1),
            endDate: new DateTime(2024, 1, 1));

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "EndDate must be after StartDate.");
    }

    [Fact]
    public void PolicyBaseValidator_WhenBasePremiumNotPositive_ShouldFail()
    {
        // Arrange
        var validator = new PolicyBaseValidator();
        var model = PolicyValidatorTestData.ValidPolicyBase(basePremiumAmount: 0m);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BasePremiumAmount" &&
                                           e.ErrorMessage == "BasePremiumAmount must be greater than 0.");
    }

    [Fact]
    public void PolicyBaseValidator_WhenBasePremiumTooManyDecimals_ShouldFail()
    {
        // Arrange
        var validator = new PolicyBaseValidator();
        var model = PolicyValidatorTestData.ValidPolicyBase(basePremiumAmount: 100.123m);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BasePremiumAmount" &&
                                           e.ErrorMessage == "BasePremiumAmount must have at most 2 decimals.");
    }

    [Fact]
    public void PolicyBaseValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new PolicyBaseValidator();
        var model = PolicyValidatorTestData.ValidPolicyBase();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // PolicyCreateRequestValidator

    [Fact]
    public void PolicyCreateRequestValidator_WhenBuildingIdInvalid_ShouldFail()
    {
        // Arrange
        var validator = new PolicyCreateRequestValidator();
        var model = PolicyValidatorTestData.ValidCreateRequest(buildingId: 0, currencyId: 1);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BuildingId" &&
                                           e.ErrorMessage == "BuildingId must be a valid id.");
    }

    [Fact]
    public void PolicyCreateRequestValidator_WhenCurrencyIdInvalid_ShouldFail()
    {
        // Arrange
        var validator = new PolicyCreateRequestValidator();
        var model = PolicyValidatorTestData.ValidCreateRequest(buildingId: 1, currencyId: 0);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CurrencyId" &&
                                           e.ErrorMessage == "CurrencyId must be a valid id.");
    }

    [Fact]
    public void PolicyCreateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new PolicyCreateRequestValidator();
        var model = PolicyValidatorTestData.ValidCreateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
