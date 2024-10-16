using AutoMapper;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ShippingMethodService : IShippingMethodService
{
    private readonly IGenericRepository<Ecommerce.Core.Domain.Entities.ShippingMethod> _repository;
    private readonly IMapper _mapper;

    public ShippingMethodService(IGenericRepository<Ecommerce.Core.Domain.Entities.ShippingMethod> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ShippingMethodDto> AddShippingMethodAsync(ShippingMethodDto shippingMethodDto)
    {
        var entity = _mapper.Map<Ecommerce.Core.Domain.Entities.ShippingMethod>(shippingMethodDto);
        await _repository.AddAsync(entity);
        await _repository.SaveAsync();
        return _mapper.Map<ShippingMethodDto>(entity);
    }

    public async Task<ShippingMethodDto> UpdateShippingMethodAsync(ShippingMethodDto shippingMethodDto)
    {
        var entity = _mapper.Map<Ecommerce.Core.Domain.Entities.ShippingMethod>(shippingMethodDto);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<ShippingMethodDto>(entity);
    }

    public async Task DeleteShippingMethodAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity != null)
        {
            await _repository.DeleteAsync(entity);
        }
    }

    public async Task<ShippingMethodDto> GetShippingMethodByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<ShippingMethodDto>(entity);
    }

    public async Task<IEnumerable<ShippingMethodDto>> GetAllShippingMethodsAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ShippingMethodDto>>(entities);
    }

    public async Task<IEnumerable<ShippingMethodDto>> GetShippingMethodsByCostRangeAsync(decimal minCost, decimal maxCost)
    {
        var entities = await _repository.FindAsync(sm => sm.Cost >= minCost && sm.Cost <= maxCost,includeword:null);
        return _mapper.Map<IEnumerable<ShippingMethodDto>>(entities);
    }

    public async Task<ShippingMethodDto> GetCheapestShippingMethodAsync()
    {
        var entity = (await _repository.GetAllAsync()).OrderBy(sm => sm.Cost).FirstOrDefault();
        return _mapper.Map<ShippingMethodDto>(entity);
    }

    public async Task<decimal> GetShippingPriceAsync(int shippingMethodId)
    {

        var entity = await _repository.GetByIdAsync(shippingMethodId);
        return entity.Cost;
    }
}
