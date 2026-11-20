using InsuranceApp.Application.Validators.Clients;
using InsuranceApp.Application.Tests.TestData;

namespace InsuranceApp.Application.Tests.Validators;

public class PagingValidatorsTests
{
    [Fact]
    public void PageRequestValidator_WhenPageTooSmall_ShouldFail()
    {
        // Arrange
        var validator = new PageRequestValidator();
        var model = PagingValidatorTestData.ValidPageRequest(page: 0, pageSize: 10);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Page" &&
                                           e.ErrorMessage == "Page number must be at least 1.");
    }

    [Fact]
    public void PageRequestValidator_WhenPageSizeTooSmall_ShouldFail()
    {
        // Arrange
        var validator = new PageRequestValidator();
        var model = PagingValidatorTestData.ValidPageRequest(page: 1, pageSize: 0);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PageSize" &&
                                           e.ErrorMessage == "Page size must be between 1 and 100.");
    }

    [Fact]
    public void PageRequestValidator_WhenPageSizeTooLarge_ShouldFail()
    {
        // Arrange
        var validator = new PageRequestValidator();
        var model = PagingValidatorTestData.ValidPageRequest(page: 1, pageSize: 101);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PageSize" &&
                                           e.ErrorMessage == "Page size must be between 1 and 100.");
    }

    [Fact]
    public void PageRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new PageRequestValidator();
        var model = PagingValidatorTestData.ValidPageRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}
