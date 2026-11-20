using InsuranceApp.Application.DTOs.Register;

namespace InsuranceApp.Application.Tests.TestData;

public static class AuthenticationValidatorTestData
{
    public static RegisterRequest ValidRegisterRequest(
        string email = "test@test.com",
        string password = "Test123!")
        => new()
        {
            Email = email,
            Password = password
        };
}
