using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Login;
using InsuranceApp.Application.DTOs.Register;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace InsuranceApp.Infrastructure.Services;

public sealed class AuthService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IConfiguration configuration,
    IUnitOfWork unitOfWork,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task RegisterAsync(
        RegisterRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Registering user {Email}.", request.Email);

        var email = request.Email.Trim();
        var broker = await _unitOfWork.Brokers.GetByEmailAsync(email, cancellationToken) 
            ?? throw new ConflictException(AuthMessages.BrokerNotFound(email));

        var user = new IdentityUser
        {
            Email = request.Email.Trim(),
            PhoneNumber = broker.Phone,
            UserName = request.Email.Trim()
        };

        var create = await _userManager.CreateAsync(user, request.Password);
        if (!create.Succeeded)
        {
            throw new BadRequestException(AuthMessages.IdentityErrors(create.Errors.Select(e => e.Description)));
        }

        broker.UserId = user.Id;
        await _unitOfWork.Brokers.UpdateAsync(broker, cancellationToken);

        var addRole = await _userManager.AddToRoleAsync(user, "Broker");
        if (!addRole.Succeeded)
        {
            throw new BadRequestException(AuthMessages.IdentityErrors(addRole.Errors.Select(e => e.Description)));
        }

        _logger.LogInformation("User registered with ID {UserId} and Broker {BrokerId}.", user.Id, broker.Id);
    }

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Login attempt for {Email}.", request.Email);

        var input = request.Email.Trim();

        var user =
            (await _userManager.FindByEmailAsync(input) ??
             await _userManager.FindByNameAsync(input))
            ?? throw new UnauthorizedException(AuthMessages.InvalidCredentials);

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains("Broker"))
        {
            var broker = await _unitOfWork.Brokers.GetByUserIdAsync(user.Id, cancellationToken)
                ?? throw new UnauthorizedException(AuthMessages.BrokerProfileNotFound);

            if (broker.Status != Domain.Enums.BrokerStatus.Active)
                throw new UnauthorizedException(AuthMessages.BrokerInactive);
        }

        var signIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!signIn.Succeeded)
            throw new UnauthorizedException(AuthMessages.InvalidCredentials);

        var token = CreateJwt(user, roles);
        return new LoginResponse(token);
    }

    private string CreateJwt(
        IdentityUser user, 
        IEnumerable<string> roles)
    {
        var jwt = _configuration.GetSection("Jwt");
        var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Email ?? user.Id),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
