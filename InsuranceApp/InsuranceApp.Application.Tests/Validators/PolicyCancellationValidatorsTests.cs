using InsuranceApp.Application.Validators.PolicyCancellation;
using InsuranceApp.Application.Tests.TestData;

namespace InsuranceApp.Application.Tests.Validators;

public class PolicyCancellationValidatorsTests
{
    [Fact]
    public void PolicyCancellationCreateRequestValidator_WhenDateDefault_ShouldFail()
    {
        // Arrange
        var validator = new PolicyCancellationCreateRequestValidator();
        var model = PolicyCancellationValidatorTestData.ValidRequest(cancellationDate: new DateTime());

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CancellationDate" &&
                                           e.ErrorMessage == "Cancellation date is required.");
    }

    [Fact]
    public void PolicyCancellationCreateRequestValidator_WhenReasonMissing_ShouldFail()
    {
        // Arrange
        var validator = new PolicyCancellationCreateRequestValidator();
        var model = PolicyCancellationValidatorTestData.ValidRequest(cancellationReason: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CancellationReason" &&
                                           e.ErrorMessage == "Cancellation reason is required.");
    }

    [Fact]
    public void PolicyCancellationCreateRequestValidator_WhenReasonTooLong_ShouldFail()
    {
        // Arrange
        var validator = new PolicyCancellationCreateRequestValidator();
        var model = PolicyCancellationValidatorTestData.ValidRequest(cancellationReason: new string('a', 501));

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CancellationReason" &&
                                           e.ErrorMessage == "Cancellation reason cannot exceed 500 characters.");
    }

    [Fact]
    public void PolicyCancellationCreateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new PolicyCancellationCreateRequestValidator();
        var model = PolicyCancellationValidatorTestData.ValidRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
