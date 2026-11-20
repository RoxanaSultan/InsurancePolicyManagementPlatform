using InsuranceApp.Application.Validators.Currency;
using InsuranceApp.Application.Tests.TestData;

namespace InsuranceApp.Application.Tests.Validators;

public class CurrencyValidatorTests
{
    // CurrencyBaseValidator

    [Fact]
    public void CurrencyBaseValidator_WhenCodeMissing_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyBaseValidator();
        var model = CurrencyValidatorTestData.ValidCurrencyBase(code: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Code" && e.ErrorMessage == "Code is required");
    }

    [Fact]
    public void CurrencyBaseValidator_WhenCodeWrongLength_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyBaseValidator();
        var model = CurrencyValidatorTestData.ValidCurrencyBase(code: "EU");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Code" &&
                                           e.ErrorMessage == "Code must have exactly 3 characters");
    }

    [Fact]
    public void CurrencyBaseValidator_WhenCodeHasNonLetters_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyBaseValidator();
        var model = CurrencyValidatorTestData.ValidCurrencyBase(code: "EU1");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Code" &&
                                           e.ErrorMessage == "Code must contain only letters");
    }

    [Fact]
    public void CurrencyBaseValidator_WhenNameMissing_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyBaseValidator();
        var model = CurrencyValidatorTestData.ValidCurrencyBase(name: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void CurrencyBaseValidator_WhenNameInvalid_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyBaseValidator();
        var model = CurrencyValidatorTestData.ValidCurrencyBase(name: "Euro1");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" &&
                                           e.ErrorMessage == "Name must contain only letters and spaces.");
    }

    [Fact]
    public void CurrencyBaseValidator_WhenExchangeRateNotPositive_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyBaseValidator();
        var model = CurrencyValidatorTestData.ValidCurrencyBase(exchangeRate: 0m);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ExchangeRateToBase" &&
                                           e.ErrorMessage == "ExchangeRateToBase must be greater than 0.");
    }

    [Fact]
    public void CurrencyBaseValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new CurrencyBaseValidator();
        var model = CurrencyValidatorTestData.ValidCurrencyBase();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // CurrencyCreateRequestValidator

    [Fact]
    public void CurrencyCreateRequestValidator_WhenBaseInvalid_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyCreateRequestValidator();
        var model = CurrencyValidatorTestData.ValidCreateRequest(code: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Code" && e.ErrorMessage == "Code is required");
    }

    [Fact]
    public void CurrencyCreateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new CurrencyCreateRequestValidator();
        var model = CurrencyValidatorTestData.ValidCreateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // CurrencyUpdateRequestValidator

    [Fact]
    public void CurrencyUpdateRequestValidator_WhenBaseInvalid_ShouldFail()
    {
        // Arrange
        var validator = new CurrencyUpdateRequestValidator();
        var model = CurrencyValidatorTestData.ValidUpdateRequest(code: "", isActive: true);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Code" && e.ErrorMessage == "Code is required");
    }

    [Fact]
    public void CurrencyUpdateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new CurrencyUpdateRequestValidator();
        var model = CurrencyValidatorTestData.ValidUpdateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
