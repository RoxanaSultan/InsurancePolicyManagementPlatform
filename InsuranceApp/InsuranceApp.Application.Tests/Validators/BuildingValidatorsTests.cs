using InsuranceApp.Application.Tests.TestData;
using InsuranceApp.Application.Validators.Buildings;

namespace InsuranceApp.Application.Tests.Validators;

public class BuildingValidatorsTests
{
    // BuildingBaseValidator

    [Fact]
    public void BuildingBaseValidator_WhenAddressNull_ShouldFail()
    {
        var validator = new BuildingBaseValidator();
        var model = BuildingValidatorTestData.ValidBuildingBase();
        model.Address = null!;

        var result = validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == "Address" &&
                 e.ErrorMessage == "Address is required.");
    }

    [Fact]
    public void BuildingBaseValidator_WhenAddressStreetMissing_ShouldFail()
    {
        var model = BuildingValidatorTestData.ValidBuildingBase(
            address: BuildingValidatorTestData.ValidAddress(street: "", number: "10"));

        var result = new BuildingBaseValidator().Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == "Address.Street" &&
                 e.ErrorMessage == "Address street is required.");
    }

    [Fact]
    public void BuildingBaseValidator_WhenCityIdMissing_ShouldFail()
    {
        var model = BuildingValidatorTestData.ValidBuildingBase(cityId: 0);

        var result = new BuildingBaseValidator().Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == "CityId" &&
                 e.ErrorMessage == "City Id is required.");
    }

    [Fact]
    public void BuildingBaseValidator_WhenAllValid_ShouldPass()
    {
        var model = BuildingValidatorTestData.ValidBuildingBase();

        var result = new BuildingBaseValidator().Validate(model);

        Assert.True(result.IsValid);
    }

    // BuildingCreateRequestValidator

    [Fact]
    public void BuildingCreateRequestValidator_WhenInvalid_ShouldFailThroughInclude()
    {
        var model = BuildingValidatorTestData.ValidCreateRequest(cityId: 0);

        var result = new BuildingCreateRequestValidator().Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == "CityId" &&
                 e.ErrorMessage == "City Id is required.");
    }

    [Fact]
    public void BuildingCreateRequestValidator_WhenValid_ShouldPass()
    {
        var model = BuildingValidatorTestData.ValidCreateRequest();

        var result = new BuildingCreateRequestValidator().Validate(model);

        Assert.True(result.IsValid);
    }

    // BuildingUpdateRequestValidator

    [Fact]
    public void BuildingUpdateRequestValidator_WhenInvalid_ShouldFailThroughInclude()
    {
        var model = BuildingValidatorTestData.ValidUpdateRequest(insuredValue: -1);

        var result = new BuildingUpdateRequestValidator().Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == "InsuredValue" &&
                 e.ErrorMessage == "Insured value must be greater than 0.");
    }

    [Fact]
    public void BuildingUpdateRequestValidator_WhenValid_ShouldPass()
    {
        var model = BuildingValidatorTestData.ValidUpdateRequest();

        var result = new BuildingUpdateRequestValidator().Validate(model);

        Assert.True(result.IsValid);
    }
}