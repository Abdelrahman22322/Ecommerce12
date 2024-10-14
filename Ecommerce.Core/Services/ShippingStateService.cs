using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

public class ShippingStateService : IShippingStateService
{
    private readonly IGenericRepository<ShippingState> _repository;
    private readonly IMapper _mapper;

    public ShippingStateService(IGenericRepository<ShippingState> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ShippingStateDto> AddShippingStateAsync(ShippingStateDto shippingStateDto)
    {
        var entity = _mapper.Map<ShippingState>(shippingStateDto);
        await _repository.AddAsync(entity);
        await _repository.SaveAsync();
        return _mapper.Map<ShippingStateDto>(entity);
    }

    public async Task<ShippingStateDto> UpdateShippingStateAsync(ShippingStateDto shippingStateDto)
    {
        var entity = _mapper.Map<ShippingState>(shippingStateDto);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<ShippingStateDto>(entity);
    }

    public async Task DeleteShippingStateAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity != null)
        {
            await _repository.DeleteAsync(entity);
        }
    }

    public async Task<ShippingStateDto> GetShippingStateByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<ShippingStateDto>(entity);
    }

    public async Task<IEnumerable<ShippingStateDto>> GetAllShippingStatesAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ShippingStateDto>>(entities);
    }

    // Business methods
    public async Task<IEnumerable<ShippingStateDto>> GetShippingStatesByStatusAsync(ShippingStatus status)
    {
        var entities = await _repository.FindAsync(ss => ss.Status == status ,includeword:null);
        return _mapper.Map<IEnumerable<ShippingStateDto>>(entities);
    }
}