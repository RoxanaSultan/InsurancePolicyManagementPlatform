using InsuranceApp.Application.Validators.FeeConfiguration;
using InsuranceApp.Application.Tests.TestData;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.Validators;

public class FeeConfigurationValidatorsTests
{
    // FeeConfigurationBaseValidator

    [Fact]
    public void FeeConfigurationBaseValidator_WhenNameMissing_ShouldFail()
    {
        // Arrange
        var validator = new FeeConfigurationBaseValidator();
        var model = FeeConfigurationValidatorTestData.ValidFeeBase(name: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required.");
    }

    [Fact]
    public void FeeConfigurationBaseValidator_WhenTypeInvalid_ShouldFail()
    {
        // Arrange
        var validator = new FeeConfigurationBaseValidator();
        var model = FeeConfigurationValidatorTestData.ValidFeeBase(type: (FeeType)999);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Type" &&
                                           e.ErrorMessage == "Type must be BrokerCommission, RiskAdjustment or AdminFee.");
    }

    [Fact]
    public void FeeConfigurationBaseValidator_WhenPercentageOutOfRange_ShouldFail()
    {
        // Arrange
        var validator = new FeeConfigurationBaseValidator();
        var model = FeeConfigurationValidatorTestData.ValidFeeBase(percentage: 0m);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Percentage" &&
                                           e.ErrorMessage == "Percentage must be between 0.01 and 100.00.");
    }

    [Fact]
    public void FeeConfigurationBaseValidator_WhenEffectiveToBeforeFrom_ShouldFail()
    {
        // Arrange
        var validator = new FeeConfigurationBaseValidator();
        var model = FeeConfigurationValidatorTestData.ValidFeeBase(
            effectiveFrom: new DateTime(2025, 1, 1),
            effectiveTo: new DateTime(2024, 1, 1));

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "EffectiveTo" &&
                                           e.ErrorMessage == "EffectiveTo must be greater than EffectiveFrom.");
    }

    [Fact]
    public void FeeConfigurationBaseValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new FeeConfigurationBaseValidator();
        var model = FeeConfigurationValidatorTestData.ValidFeeBase();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // FeeConfigurationCreateRequestValidator

    [Fact]
    public void FeeConfigurationCreateRequestValidator_WhenBaseInvalid_ShouldFail()
    {
        // Arrange
        var validator = new FeeConfigurationCreateRequestValidator();
        var model = FeeConfigurationValidatorTestData.ValidCreateRequest(name: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required.");
    }

    [Fact]
    public void FeeConfigurationCreateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new FeeConfigurationCreateRequestValidator();
        var model = FeeConfigurationValidatorTestData.ValidCreateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // FeeConfigurationUpdateRequestValidator

    [Fact]
    public void FeeConfigurationUpdateRequestValidator_WhenBaseInvalid_ShouldFail()
    {
        // Arrange
        var validator = new FeeConfigurationUpdateRequestValidator();
        var model = FeeConfigurationValidatorTestData.ValidUpdateRequest(name: "", isActive: true);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required.");
    }

    [Fact]
    public void FeeConfigurationUpdateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new FeeConfigurationUpdateRequestValidator();
        var model = FeeConfigurationValidatorTestData.ValidUpdateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
