using AutoMapper;
using InsuranceApp.Application.DTOs.PolicyCancellation;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Policy;
using InsuranceApp.Domain.Entities.Policy;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Services.Policy;

public class PolicyCancellationService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    ILogger<PolicyCancellationService> logger
    ) : IPolicyCancellationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<PolicyCancellationService> _logger = logger;

    public async Task<PolicyCancellationResponse> CreateAsync(
        int policyId, 
        PolicyCancellationCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating policy cancellation for Policy {PolicyId}.", policyId);

        var policyCancellation = _mapper.Map<PolicyCancellation>(request);
        policyCancellation.PolicyId = policyId;

        await _unitOfWork.PolicyCancellations.AddAsync(policyCancellation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Policy cancellation created with ID {PolicyCancellationId} for Policy {PolicyId}.", policyCancellation.Id, policyId);

        return _mapper.Map<PolicyCancellationResponse>(policyCancellation);
    }
}
