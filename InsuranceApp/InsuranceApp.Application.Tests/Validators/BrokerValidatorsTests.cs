using InsuranceApp.Application.Validators.Broker;
using InsuranceApp.Application.Tests.TestData;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.Tests.Validators;

public class BrokerValidatorTests
{
    // BrokerBaseValidator

    [Fact]
    public void BrokerBaseValidator_WhenNameMissing_ShouldFail()
    {
        // Arrange
        var validator = new BrokerBaseValidator();
        var model = BrokerValidatorTestData.ValidBrokerBase(name: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required");
    }

    [Fact]
    public void BrokerBaseValidator_WhenNameHasInvalidCharacters_ShouldFail()
    {
        // Arrange
        var validator = new BrokerBaseValidator();
        var model = BrokerValidatorTestData.ValidBrokerBase(name: "John1");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" &&
                                           e.ErrorMessage == "Name must contain only letters and spaces.");
    }

    [Fact]
    public void BrokerBaseValidator_WhenEmailInvalid_ShouldFail()
    {
        // Arrange
        var validator = new BrokerBaseValidator();
        var model = BrokerValidatorTestData.ValidBrokerBase(email: "not-an-email");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email" &&
                                           e.ErrorMessage == "Email must be a valid email address.");
    }

    [Fact]
    public void BrokerBaseValidator_WhenPhoneInvalid_ShouldFail()
    {
        // Arrange
        var validator = new BrokerBaseValidator();
        var model = BrokerValidatorTestData.ValidBrokerBase(phone: "123");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Phone" &&
                                           e.ErrorMessage == "Phone number must be Romanian: exactly 10 digits and start with 0.");
    }

    [Fact]
    public void BrokerBaseValidator_WhenCommissionNegative_ShouldFail()
    {
        // Arrange
        var validator = new BrokerBaseValidator();
        var model = BrokerValidatorTestData.ValidBrokerBase(commission: -1m);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CommissionPercentage" &&
                                           e.ErrorMessage == "Commission percentage greater than or equal to 0.");
    }

    [Fact]
    public void BrokerBaseValidator_WhenAllValid_ShouldPass()
    {
        // Arrange
        var validator = new BrokerBaseValidator();
        var model = BrokerValidatorTestData.ValidBrokerBase();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // BrokerCreateRequestValidator

    [Fact]
    public void BrokerCreateRequestValidator_WhenStatusInvalid_ShouldFail()
    {
        // Arrange
        var validator = new BrokerCreateRequestValidator();
        var model = BrokerValidatorTestData.ValidCreateRequest(status: (BrokerStatus)999);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Status" &&
                                           e.ErrorMessage == "Broker status must be between Active and Inactive.");
    }

    [Fact]
    public void BrokerCreateRequestValidator_WhenStatusValid_ShouldPass()
    {
        // Arrange
        var validator = new BrokerCreateRequestValidator();
        var model = BrokerValidatorTestData.ValidCreateRequest(status: BrokerStatus.Active);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // BrokerUpdateRequestValidator

    [Fact]
    public void BrokerUpdateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new BrokerUpdateRequestValidator();
        var model = BrokerValidatorTestData.ValidUpdateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
