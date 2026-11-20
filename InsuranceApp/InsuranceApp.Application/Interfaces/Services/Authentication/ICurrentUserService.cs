namespace InsuranceApp.Application.Interfaces.Services.Authentication;

public interface ICurrentUserService
{
    string? GetUserId();
}