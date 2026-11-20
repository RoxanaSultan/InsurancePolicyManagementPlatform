using InsuranceApp.Application.DTOs.RiskFactorConfiguration;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Interfaces.Services.Metadata;

public interface IRiskFactorConfigurationService
{
    Task<PageResponse<RiskFactorConfigurationResponse>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<RiskFactorConfigurationResponse> CreateAsync(
        RiskFactorConfigurationCreateRequest request, 
        CancellationToken cancellationToken);
    Task<RiskFactorConfigurationResponse> UpdateAsync(
        int riskFactorConfigurationId, 
        RiskFactorConfigurationUpdateRequest request, 
        CancellationToken cancellationToken);
}