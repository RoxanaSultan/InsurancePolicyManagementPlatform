using AutoMapper;
using InsuranceApp.Application.Common.Messages;
using InsuranceApp.Application.DTOs.Buildings;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Exceptions;
using InsuranceApp.Application.Interfaces.Repositories;
using InsuranceApp.Application.Interfaces.Services.Building;
using Microsoft.Extensions.Logging;
using BuildingEntity = InsuranceApp.Domain.Entities.Building.Building;

namespace InsuranceApp.Application.Services.Building;

public class BuildingService(
    IUnitOfWork unitOfWork, 
    IMapper mapper,
    ILogger<BuildingService> logger) : IBuildingService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<BuildingService> _logger = logger;

    public async Task<BuildingResponse> CreateAsync(
        int clientId, 
        BuildingCreateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating building for Client {ClientId}.", clientId);

        _ = await _unitOfWork.Clients.GetByIdAsync(clientId, cancellationToken) 
            ?? throw new NotFoundException(BuildingMessages.ClientNotFound(clientId));
        _ = await _unitOfWork.Cities.GetByIdAsync(request.CityId, cancellationToken)
            ?? throw new NotFoundException(BuildingMessages.CityNotFound(request.CityId));

        var building = _mapper.Map<BuildingEntity>(request);
        building.ClientId = clientId;

        await _unitOfWork.Buildings.AddAsync(building, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Building created with ID {BuildingId} for Client {ClientId}.", building.Id, clientId);

        return _mapper.Map<BuildingResponse>(building);
    }

    public async Task<PageResponse<BuildingResponse>> GetByClientIdAsync(
        int clientId, 
        PageRequest pageRequest, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting buildings for Client {ClientId}.", clientId);

        _ = await _unitOfWork.Clients.GetByIdAsync(clientId, cancellationToken) 
            ?? throw new NotFoundException(BuildingMessages.ClientNotFound(clientId));

        var buildings = await _unitOfWork.Buildings.GetByClientIdAsync(clientId, pageRequest, cancellationToken);

        var mappedItems = _mapper.Map<IReadOnlyList<BuildingResponse>>(buildings.Items);

        return new PageResponse<BuildingResponse>
        {
            Items = mappedItems,
            Page = buildings.Page,
            PageSize = buildings.PageSize,
            TotalItems = buildings.TotalItems,
            TotalPages = buildings.TotalPages
        };
    }

    public async Task<BuildingDetailsResponse> GetByIdAsync(
        int buildingId, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting building {BuildingId}.", buildingId);

        var building = await _unitOfWork.Buildings.GetByIdAsync(buildingId, cancellationToken) 
            ?? throw new NotFoundException(BuildingMessages.BuildingNotFound(buildingId));

        return _mapper.Map<BuildingDetailsResponse>(building);
    }

    public async Task<BuildingResponse> UpdateAsync(
        int buildingId, 
        BuildingUpdateRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating building {BuildingId}.", buildingId);

        var existingBuilding = await _unitOfWork.Buildings.GetByIdAsync(buildingId, cancellationToken) 
            ?? throw new NotFoundException(BuildingMessages.BuildingNotFound(buildingId));

        _mapper.Map(request, existingBuilding);
        await _unitOfWork.Buildings.UpdateAsync(existingBuilding, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BuildingResponse>(existingBuilding);
    }
}
