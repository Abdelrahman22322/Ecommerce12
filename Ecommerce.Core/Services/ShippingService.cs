using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ShippingService : IShippingService
{
    private readonly IGenericRepository<Shipping> _shippingRepository;
    private readonly Dictionary<ShippingMethod, decimal> _shippingCosts;
    private readonly HashSet<ShippingMethod> _disabledMethods;
    private readonly IMapper _mapper;

    public ShippingService(IGenericRepository<Shipping> shippingRepository, IMapper mapper)
    {
        _shippingRepository = shippingRepository;
        _mapper = mapper;
        _shippingCosts = new Dictionary<ShippingMethod, decimal>
        {
            { ShippingMethod.Standard, 10m },
            { ShippingMethod.Express, 20m },
            { ShippingMethod.Overnight, 30m }
        };
        _disabledMethods = new HashSet<ShippingMethod>();
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
        var shippings = await _shippingRepository.FindAsync1(s => s.Status == status, null); // Provide the includeword parameter
        return _mapper.Map<IEnumerable<ShippingDto>>(shippings);
    }

    public async Task<ShippingDto> GetShippingByTrackingCodeAsync(string trackingCode)
    {
        var shipping = await _shippingRepository.FindAsync1(s => s.TrackingNumber == trackingCode, null); // Provide the includeword parameter
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
        var shipping = await _shippingRepository.GetByIdAsync(shippingId);
        if (shipping != null)
        {
            shipping.Method = method;
            shipping.Price = cost;
            await _shippingRepository.UpdateAsync(shipping);
        }
    }

    public async Task AssignShippingToOrderAsync(int orderId, int shipperId, ShippingMethod method, decimal cost, string trackingCode)
    {
        var shipping = new Shipping
        {
            Orders = new List<Order> { new Order { Id = orderId } }, // Assuming Order has an Id property
            ShipperId = shipperId,
            Method = method,
            Price = cost,
            TrackingNumber = trackingCode,
            Status = ShippingStatus.Pending // Set initial status
        };

        await _shippingRepository.AddAsync(shipping);
        await _shippingRepository.SaveAsync();
    }

    public async Task<ShippingStatus> GetShippingStatusByTrackingCodeAsync(string trackingCode)
    {
        var shipping = await _shippingRepository.FindAsync1(s => s.TrackingNumber == trackingCode, null); // Provide the includeword parameter
        if (shipping != null)
        {
            return shipping.Status;
        }
        throw new KeyNotFoundException("Shipping not found with the provided tracking code.");
    }

    public void AssignShippingCost(ShippingMethod method, decimal cost)
    {
        if (_shippingCosts.ContainsKey(method))
        {
            _shippingCosts[method] = cost;
        }
        else
        {
            _shippingCosts.Add(method, cost);
        }
    }

    public decimal GetShippingPrice(ShippingMethod method)
    {
        if (_disabledMethods.Contains(method))
        {
            throw new InvalidOperationException($"Shipping method {method} is disabled.");
        }

        return _shippingCosts.TryGetValue(method, out var cost) ? cost : throw new ArgumentOutOfRangeException(nameof(method), method, null);
    }

    public void DisableShippingMethod(ShippingMethod method)
    {
        _disabledMethods.Add(method);
    }

    public void EnableShippingMethod(ShippingMethod method)
    {
        _disabledMethods.Remove(method);
    }

    public Task<bool> IsValidShippingMethodAsync(ShippingMethod method)
    {
        return Task.FromResult(Enum.IsDefined(typeof(ShippingMethod), method));
    }
}
