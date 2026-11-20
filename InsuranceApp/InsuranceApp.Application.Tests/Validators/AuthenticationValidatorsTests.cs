using InsuranceApp.Application.Validators.Authentication;
using InsuranceApp.Application.Tests.TestData;

namespace InsuranceApp.Application.Tests.Validators;

public class AuthenticationValidatorTests
{
    [Fact]
    public void RegisterRequestValidator_WhenEmailMissing_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(email: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage == "Email is required.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenEmailInvalid_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(email: "invalid");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email" &&
                                           e.ErrorMessage == "Email must be a valid email address.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenPasswordMissing_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(password: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage == "Password is required.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenPasswordTooShort_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(password: "T1!a");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" &&
                                           e.ErrorMessage == "Password must be at least 8 characters long.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenPasswordMissingLowercase_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(password: "TEST123!");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" &&
                                           e.ErrorMessage == "Password must contain at least one lowercase letter.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenPasswordMissingUppercase_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(password: "test123!");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" &&
                                           e.ErrorMessage == "Password must contain at least one uppercase letter.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenPasswordMissingDigit_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(password: "Test!!!!");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" &&
                                           e.ErrorMessage == "Password must contain at least one digit.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenPasswordMissingSpecialCharacter_ShouldFail()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest(password: "Test1234");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" &&
                                           e.ErrorMessage == "Password must contain at least one special character.");
    }

    [Fact]
    public void RegisterRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new RegisterRequestValidator();
        var model = AuthenticationValidatorTestData.ValidRegisterRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
