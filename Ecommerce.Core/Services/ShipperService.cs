using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ShipperService : IShipperService
{
    private readonly IGenericRepository<Shipper> _shipperRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ShipperService> _logger;

    public ShipperService(IGenericRepository<Shipper> shipperRepository, IMapper mapper, ILogger<ShipperService> logger)
    {
        _shipperRepository = shipperRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ShipperDto> CreateShipperAsync(ShipperDto shipperDto)
    {
        try
        {
            var shipper = _mapper.Map<Shipper>(shipperDto);
            await _shipperRepository.AddAsync(shipper);
            await _shipperRepository.SaveAsync();
            return _mapper.Map<ShipperDto>(shipper);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the shipper.");
            throw new Exception("An error occurred while creating the shipper. Please try again later.", ex);
        }
    }

    public async Task<ShipperDto> GetShipperByIdAsync(int shipperId)
    {
        try
        {
            var shipper = await _shipperRepository.GetByIdAsync(shipperId);
            return _mapper.Map<ShipperDto>(shipper);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the shipper with ID {shipperId}.");
            throw new Exception("An error occurred while retrieving the shipper. Please try again later.", ex);
        }
    }

    public async Task<IEnumerable<ShipperDto>> GetAllShippersAsync()
    {
        try
        {
            var shippers = await _shipperRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShipperDto>>(shippers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all shippers.");
            throw new Exception("An error occurred while retrieving all shippers. Please try again later.", ex);
        }
    }

    public async Task UpdateShipperAsync(ShipperDto shipperDto)
    {
        try
        {
            var shipper = _mapper.Map<Shipper>(shipperDto);
            await _shipperRepository.UpdateAsync(shipper);
            await _shipperRepository.SaveAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the shipper with ID {shipperDto.Id}.");
            throw new Exception("An error occurred while updating the shipper. Please try again later.", ex);
        }
    }

    public async Task DeleteShipperAsync(int shipperId)
    {
        try
        {
            var shipper = await _shipperRepository.GetByIdAsync(shipperId);
            if (shipper != null)
            {
                await _shipperRepository.DeleteAsync(shipper);
                await _shipperRepository.SaveAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the shipper with ID {ShipperId}", shipperId);
            throw new Exception("An error occurred while deleting the shipper. Please try again later.", ex);
        }
    }

    public async Task<ShipperDto> AssignOrderToLeastAssignedShipper()
    {
        try
        {
            var shippers = await _shipperRepository.GetAllAsync(includeword: "Orders");
            var leastAssignedShipper = shippers.OrderBy(s => s.AssignedOrdersCount).FirstOrDefault();
            if (leastAssignedShipper == null)
            {
                throw new Exception("No shippers available.");
            }

            // Increment the AssignedOrdersCount
            leastAssignedShipper.AssignedOrdersCount++;
            await _shipperRepository.UpdateAsync(leastAssignedShipper);
            await _shipperRepository.SaveAsync();

            return _mapper.Map<ShipperDto>(leastAssignedShipper);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while assigning the order to the least assigned shipper.");
            throw new Exception("An error occurred while assigning the order to the least assigned shipper. Please try again later.", ex);
        }
    }
}
