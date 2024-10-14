using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

public class ShipperService : IShipperService
{
    private readonly IGenericRepository<Shipper> _shipperRepository;
    private readonly IMapper _mapper;

    public ShipperService(IGenericRepository<Shipper> shipperRepository, IMapper mapper)
    {
        _shipperRepository = shipperRepository;
        _mapper = mapper;
    }

    public async Task<ShipperDto> CreateShipperAsync(ShipperDto shipperDto)
    {
        var shipper = _mapper.Map<Shipper>(shipperDto);
        await _shipperRepository.AddAsync(shipper);
        await _shipperRepository.SaveAsync();
        return _mapper.Map<ShipperDto>(shipper);
    }

    public async Task<ShipperDto> GetShipperByIdAsync(int shipperId)
    {
        var shipper = await _shipperRepository.GetByIdAsync(shipperId);
        return _mapper.Map<ShipperDto>(shipper);
    }

    public async Task<IEnumerable<ShipperDto>> GetAllShippersAsync()
    {
        var shippers = await _shipperRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ShipperDto>>(shippers);
    }

   
    public async Task UpdateShipperAsync(ShipperDto shipperDto)
    {
        var shipper = _mapper.Map<Shipper>(shipperDto);
        await _shipperRepository.UpdateAsync(shipper);
    }

    public async Task DeleteShipperAsync(int shipperId)
    {
        var shipper = await _shipperRepository.GetByIdAsync(shipperId);
        if (shipper != null)
        {
            await _shipperRepository.DeleteAsync(shipper);
        }
    }

    
    
    public async Task<ShipperDto> AssignOrderToLeastAssignedShipper()
    {
        var shippers = await _shipperRepository.GetAllAsync(includeword: "Orders");
        var leastAssignedShipper = shippers.OrderBy(s => s.Orders?.Count ?? 0).FirstOrDefault();
        if (leastAssignedShipper == null)
        {
            throw new Exception("No shippers available.");
        }
        return _mapper.Map<ShipperDto>(leastAssignedShipper);
    }

}