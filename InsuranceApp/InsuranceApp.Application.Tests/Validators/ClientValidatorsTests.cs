using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.Tests.TestData;
using InsuranceApp.Application.Validators.Clients;
using InsuranceApp.Domain.Enums;
using InsuranceApp.Domain.ValueObjects;

namespace InsuranceApp.Application.Tests.Validators;

public class ClientValidatorsTests
{
    // ClientBaseValidator

    [Fact]
    public void ClientBaseValidator_WhenNameMissing_ShouldFail()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase(name: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required.");
    }

    [Fact]
    public void ClientBaseValidator_WhenNameTooShort_ShouldFail()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase(name: "A");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Name must be at least 2 characters.");
    }

    [Fact]
    public void ClientBaseValidator_WhenContactInfoNull_ShouldFail()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase();
        model.ContactInfo = null!;

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ContactInfo" && e.ErrorMessage == "Contact information is required.");
    }

    [Fact]
    public void ClientBaseValidator_WhenEmailInvalid_ShouldFail()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase(email: "not-an-email");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ContactInfo.Email" && e.ErrorMessage == "Email must be a valid email address.");
    }

    [Fact]
    public void ClientBaseValidator_WhenPhoneInvalid_ShouldFail()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase(phone: "123");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ContactInfo.PhoneNumber" &&
                                           e.ErrorMessage == "Phone number must be Romanian: exactly 10 digits and start with 0.");
    }

    [Fact]
    public void ClientBaseValidator_WhenAddressProvided_StreetMissing_ShouldFail()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase(address: new Address { Street = "", Number = "10" });

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ContactInfo.Address.Street" && e.ErrorMessage == "Street is required.");
    }

    [Fact]
    public void ClientBaseValidator_WhenAddressProvided_NumberMissing_ShouldFail()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase(address: new Address { Street = "Main", Number = "" });

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ContactInfo.Address.Number" && e.ErrorMessage == "Number is required.");
    }

    [Fact]
    public void ClientBaseValidator_WhenAllValid_ShouldPass()
    {
        // Arrange
        var validator = new ClientBaseValidator();
        var model = ClientValidatorTestData.ValidClientBase();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // ClientCreateRequestValidator

    [Fact]
    public void ClientCreateRequestValidator_WhenClientTypeInvalid_ShouldFail()
    {
        // Arrange
        var validator = new ClientCreateRequestValidator();
        var model = ClientValidatorTestData.ValidCreateRequest(clientType: (ClientType)999);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ClientType" &&
                                           e.ErrorMessage == "Client type must be between Individual and Company.");
    }

    [Fact]
    public void ClientCreateRequestValidator_WhenIdentificationNumberMissing_ShouldFail()
    {
        // Arrange
        var validator = new ClientCreateRequestValidator();
        var model = ClientValidatorTestData.ValidCreateRequest(
            clientType: ClientType.Individual,
            identificationNumber: "");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdentificationNumber" &&
                                           e.ErrorMessage == "Identification number is required.");
    }

    [Fact]
    public void ClientCreateRequestValidator_WhenIndividualAndCnpNot13Digits_ShouldFail()
    {
        // Arrange
        var validator = new ClientCreateRequestValidator();
        var model = ClientValidatorTestData.ValidCreateRequest(
            clientType: ClientType.Individual,
            identificationNumber: "123");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdentificationNumber" &&
                                           e.ErrorMessage == "CNP must contain exactly 13 digits.");
    }

    [Fact]
    public void ClientCreateRequestValidator_WhenCompanyAndIdHasNonDigits_ShouldFail()
    {
        // Arrange
        var validator = new ClientCreateRequestValidator();
        var model = ClientValidatorTestData.ValidCreateRequest(
            name: "ACME",
            clientType: ClientType.Company,
            identificationNumber: "ABC123");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdentificationNumber" &&
                                           e.ErrorMessage == "Company Registration Number must contain only digits.");
    }

    [Fact]
    public void ClientCreateRequestValidator_WhenCompanyAndIdTooShort_ShouldFail()
    {
        // Arrange
        var validator = new ClientCreateRequestValidator();
        var model = ClientValidatorTestData.ValidCreateRequest(
            name: "ACME",
            clientType: ClientType.Company,
            identificationNumber: "1");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdentificationNumber" &&
                                           e.ErrorMessage == "Company Registration Number must be at least 2 digits.");
    }

    [Fact]
    public void ClientCreateRequestValidator_WhenValidIndividual_ShouldPass()
    {
        // Arrange
        var validator = new ClientCreateRequestValidator();
        var model = ClientValidatorTestData.ValidCreateRequest(
            name: "John Doe",
            clientType: ClientType.Individual,
            identificationNumber: "1234567890123");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ClientCreateRequestValidator_WhenValidCompany_ShouldPass()
    {
        // Arrange
        var validator = new ClientCreateRequestValidator();
        var model = ClientValidatorTestData.ValidCreateRequest(
            name: "ACME",
            clientType: ClientType.Company,
            identificationNumber: "12");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // ClientUpdateRequestValidator

    [Fact]
    public void ClientUpdateRequestValidator_WhenIndividualAndCnpInvalid_ShouldFail()
    {
        // Arrange
        var validator = new ClientUpdateRequestValidator();
        var model = ClientValidatorTestData.ValidUpdateRequest(
            name: "John Doe",
            clientType: ClientType.Individual,
            identificationNumber: "123");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdentificationNumber" &&
                                           e.ErrorMessage == "CNP must contain exactly 13 digits.");
    }

    [Fact]
    public void ClientUpdateRequestValidator_WhenCompanyAndIdHasNonDigits_ShouldFail()
    {
        // Arrange
        var validator = new ClientUpdateRequestValidator();
        var model = ClientValidatorTestData.ValidUpdateRequest(
            name: "ACME",
            clientType: ClientType.Company,
            identificationNumber: "ABC");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdentificationNumber" &&
                                           e.ErrorMessage == "Company Registration Number must contain only digits.");
    }

    [Fact]
    public void ClientUpdateRequestValidator_WhenValid_ShouldPass()
    {
        // Arrange
        var validator = new ClientUpdateRequestValidator();
        var model = ClientValidatorTestData.ValidUpdateRequest();

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    // ClientSearchRequestValidator

    [Fact]
    public void ClientSearchRequestValidator_WhenNameProvidedButTooShort_ShouldFail()
    {
        // Arrange
        var validator = new ClientSearchRequestValidator();
        var model = ClientValidatorTestData.ValidSearchRequest(name: "A", identificationNumber: null);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void ClientSearchRequestValidator_WhenIdentificationNumberProvidedButTooShort_ShouldFail()
    {
        // Arrange
        var validator = new ClientSearchRequestValidator();
        var model = ClientValidatorTestData.ValidSearchRequest(name: null, identificationNumber: "1234");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "IdentificationNumber");
    }

    [Fact]
    public void ClientSearchRequestValidator_WhenValidWithName_ShouldPass()
    {
        // Arrange
        var validator = new ClientSearchRequestValidator();
        var model = ClientValidatorTestData.ValidSearchRequest(name: "John", identificationNumber: null);

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ClientSearchRequestValidator_WhenValidWithIdentificationNumber_ShouldPass()
    {
        // Arrange
        var validator = new ClientSearchRequestValidator();
        var model = ClientValidatorTestData.ValidSearchRequest(name: null, identificationNumber: "12345");

        // Act
        var result = validator.Validate(model);

        // Assert
        Assert.True(result.IsValid);
    }
}