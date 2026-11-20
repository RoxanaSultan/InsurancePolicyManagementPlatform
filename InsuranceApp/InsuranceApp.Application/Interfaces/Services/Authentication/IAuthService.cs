using InsuranceApp.Application.DTOs.Login;
using InsuranceApp.Application.DTOs.Register;

namespace InsuranceApp.Application.Interfaces.Services.Authentication;

public interface IAuthService
{
    Task RegisterAsync(
        RegisterRequest request, 
        CancellationToken cancellationToken = default);
    Task<LoginResponse> LoginAsync(
        LoginRequest request, 
        CancellationToken cancellationToken = default);
}