using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShippingMethod = Ecommerce.Core.Domain.Enums.ShippingMethod;

public class ShippingService : IShippingService
{
    private readonly IGenericRepository<Shipping> _shippingRepository;
    private readonly IShippingStateService _shippingStateService;
    private readonly IShippingMethodService _shippingMethodService;
    private readonly IMapper _mapper;

    public ShippingService(
        IGenericRepository<Shipping> shippingRepository,
        IShippingStateService shippingStateService,
        IShippingMethodService shippingMethodService,
        IMapper mapper)
    {
        _shippingRepository = shippingRepository;
        _shippingStateService = shippingStateService;
        _shippingMethodService = shippingMethodService;
        _mapper = mapper;
    }

    public async Task<ShippingDto> CreateShippingAsync(ShippingDto shippingDto)
    {
        var shipping = _mapper.Map<Shipping>(shippingDto);
        await _shippingRepository.AddAsync(shipping);
        await _shippingRepository.SaveAsync();
        return _mapper.Map<ShippingDto>(shipping);
    }

    public async Task<ShippingDto> GetShippingByIdAsync(int shippingId)
    {
        var shipping = await _shippingRepository.GetByIdAsync(shippingId);
        return _mapper.Map<ShippingDto>(shipping);
    }

    public async Task<IEnumerable<ShippingDto>> GetShippingsByStatusAsync(ShippingStatus status)
    {
        var shippings = await _shippingRepository.FindAsync1(s => s.Status == status, null);
        return _mapper.Map<IEnumerable<ShippingDto>>(shippings);
    }

    public async Task<ShippingDto> GetShippingByTrackingCodeAsync(string trackingCode)
    {
        var shipping = await _shippingRepository.FindAsync1(s => s.TrackingNumber == trackingCode, null);
        return _mapper.Map<ShippingDto>(shipping);
    }

    public async Task UpdateShippingStatusAsync(int shippingId, ShippingStatus newStatus)
    {
        var shipping = await _shippingRepository.GetByIdAsync(shippingId);
        if (shipping != null)
        {
            shipping.Status = newStatus;
            await _shippingRepository.UpdateAsync(shipping);
        }
    }

    public async Task AssignShippingMethodAsync(int shippingId, ShippingMethod method, decimal cost)
    {
        throw new NotImplementedException();
    }

    public async Task AssignShippingToOrderAsync(int orderId, int shipperId, ShippingMethod method, decimal cost, string trackingCode)
    {
        throw new NotImplementedException();
    }

    public async Task AssignShippingMethodAsync(int shippingId, int methodId)
    {
        var shipping = await _shippingRepository.GetByIdAsync(shippingId);
        if (shipping != null)
        {
            var method = await _shippingMethodService.GetShippingMethodByIdAsync(methodId);
            if (method != null)
            {
                shipping.Method = (ShippingMethod)method.Id;
                shipping.Price = method.Cost;
                await _shippingRepository.UpdateAsync(shipping);
            }
        }
    }

    public async Task AssignShippingToOrderAsync(int orderId, int shipperId, int methodId, string trackingCode)
    {
        var method = await _shippingMethodService.GetShippingMethodByIdAsync(methodId);
        if (method != null)
        {
            var shipping = new Shipping
            {
                Orders = new List<Order> { new Order { Id = orderId } },
                ShipperId = shipperId,
                Method = (ShippingMethod)method.Id,
                Price = method.Cost,
                TrackingNumber = trackingCode,
                Status = ShippingStatus.Pending
            };

            await _shippingRepository.AddAsync(shipping);
            await _shippingRepository.SaveAsync();
        }
    }

    public async Task<ShippingStatus> GetShippingStatusByTrackingCodeAsync(string trackingCode)
    {
        var shipping = await _shippingRepository.FindAsync1(s => s.TrackingNumber == trackingCode, null);
        if (shipping != null)
        {
            return shipping.Status;
        }
        throw new KeyNotFoundException("Shipping not found with the provided tracking code.");
    }

    public void AssignShippingCost(ShippingMethod method, decimal cost)
    {
        throw new NotImplementedException();
    }

    public decimal GetShippingPrice(ShippingMethod method)
    {
        throw new NotImplementedException();
    }

    public void DisableShippingMethod(ShippingMethod method)
    {
        throw new NotImplementedException();
    }

    public void EnableShippingMethod(ShippingMethod method)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsValidShippingMethodAsync(ShippingMethod method)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsValidShippingMethodAsync(int methodId)
    {
        var method = await _shippingMethodService.GetShippingMethodByIdAsync(methodId);
        return method != null;
    }
}
