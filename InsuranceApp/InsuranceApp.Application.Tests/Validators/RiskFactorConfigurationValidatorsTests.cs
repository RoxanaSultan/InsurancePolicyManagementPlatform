using InsuranceApp.Application.Validators.RiskFactorConfiguration;
using InsuranceApp.Application.Tests.TestData;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.Validators;

public class RiskFactorConfigurationValidatorsTests
{
    // RiskFactorConfigurationBaseValidator

    [Fact]
    public void RiskFactorConfigurationBaseValidator_WhenLevelInvalid_ShouldFail()
    {
        // Arrange
        var validator = new RiskFactorConfigurationBaseValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidRiskFactorBase(level: (RiskFactorLevel)999);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Level" &&
                                           e.ErrorMessage == "Level must be between Country, County, City and BuildingType.");
    }

    [Fact]
    public void RiskFactorConfigurationBaseValidator_WhenReferenceIdMissing_ShouldFail()
    {
        // Arrange
        var validator = new RiskFactorConfigurationBaseValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidRiskFactorBase(referenceId: 0);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ReferenceId" &&
                                           e.ErrorMessage == "ReferenceId is required.");
    }

    [Fact]
    public void RiskFactorConfigurationBaseValidator_WhenAdjustmentOutOfRange_ShouldFail()
    {
        // Arrange
        var validator = new RiskFactorConfigurationBaseValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidRiskFactorBase(adjustmentPercentage: 150m);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "AdjustmentPercentage" &&
                                           e.ErrorMessage == "AdjustmentPercentage must be between -100 and 100.");
    }

    [Fact]
    public void RiskFactorConfigurationBaseValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new RiskFactorConfigurationBaseValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidRiskFactorBase();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // RiskFactorConfigurationCreateRequestValidator

    [Fact]
    public void RiskFactorConfigurationCreateRequestValidator_WhenBaseInvalid_ShouldFail()
    {
        // Arrange
        var validator = new RiskFactorConfigurationCreateRequestValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidCreateRequest(level: (RiskFactorLevel)999);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Level" &&
                                           e.ErrorMessage == "Level must be between Country, County, City and BuildingType.");
    }

    [Fact]
    public void RiskFactorConfigurationCreateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new RiskFactorConfigurationCreateRequestValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidCreateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // RiskFactorConfigurationUpdateRequestValidator

    [Fact]
    public void RiskFactorConfigurationUpdateRequestValidator_WhenBaseInvalid_ShouldFail()
    {
        // Arrange
        var validator = new RiskFactorConfigurationUpdateRequestValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidUpdateRequest(
            level: RiskFactorLevel.County,
            referenceId: 0,
            isActive: true);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ReferenceId" &&
                                           e.ErrorMessage == "ReferenceId is required.");
    }

    [Fact]
    public void RiskFactorConfigurationUpdateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new RiskFactorConfigurationUpdateRequestValidator();
        var model = RiskFactorConfigurationValidatorTestData.ValidUpdateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
