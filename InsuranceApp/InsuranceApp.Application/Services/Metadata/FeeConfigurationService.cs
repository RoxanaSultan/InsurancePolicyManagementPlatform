using AutoMapper;
using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.FeeConfiguration;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Metadata;
using InsuranceApp.Domain.Entities.Metadata;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Application.Services.Metadata;

public class FeeConfigurationService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    ILogger<FeeConfigurationService> logger
    ) : IFeeConfigurationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<FeeConfigurationService> _logger = logger;

    public async Task<PageResponse<FeeConfigurationResponse>> GetAllAsync(
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting fee configurations.");

        var feeConfigurations = await _unitOfWork.FeeConfigurations.GetAllAsync(pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<FeeConfigurationResponse>>(feeConfigurations.Items);

        return new PageResponse<FeeConfigurationResponse>
        {
            Items = mappedItems,
            Page = feeConfigurations.Page,
            PageSize = feeConfigurations.PageSize,
            TotalItems = feeConfigurations.TotalItems,
            TotalPages = feeConfigurations.TotalPages
        };
    }

    public async Task<FeeConfigurationResponse> CreateAsync(
        FeeConfigurationCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating fee configuration.");

        var feeConfiguration = _mapper.Map<FeeConfiguration>(request);
        feeConfiguration.IsActive = true;

        await _unitOfWork.FeeConfigurations.AddAsync(feeConfiguration, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Fee configuration created with ID {FeeConfigurationId}.", feeConfiguration.Id);

        return _mapper.Map<FeeConfigurationResponse>(feeConfiguration);
    }

    public async Task<FeeConfigurationResponse> UpdateAsync(
        int feeConfigurationId, 
        FeeConfigurationUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating fee configuration {FeeConfigurationId}.", feeConfigurationId);

        var existingFeeConfiguration = await _unitOfWork.FeeConfigurations.GetByIdAsync(feeConfigurationId, cancellationToken) 
            ?? throw new NotFoundException(FeeConfigurationMessages.NotFound(feeConfigurationId));

        _mapper.Map(request, existingFeeConfiguration);
        
        await _unitOfWork.FeeConfigurations.UpdateAsync(existingFeeConfiguration, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<FeeConfigurationResponse>(existingFeeConfiguration);
    }
}
