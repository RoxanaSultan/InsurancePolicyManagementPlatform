using InsuranceApp.Application.DTOs.FeeConfiguration;
using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Interfaces.Services.Metadata;

public interface IFeeConfigurationService
{
    Task<PageResponse<FeeConfigurationResponse>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken);
    Task<FeeConfigurationResponse> CreateAsync(
        FeeConfigurationCreateRequest request, 
        CancellationToken cancellationToken);
    Task<FeeConfigurationResponse> UpdateAsync(
        int feeConfigurationId, 
        FeeConfigurationUpdateRequest request, 
        CancellationToken cancellationToken);
}
