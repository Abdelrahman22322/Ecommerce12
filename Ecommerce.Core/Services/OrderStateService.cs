using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

public class OrderStateService : IOrderStateService
{
    private readonly IGenericRepository<OrderStatus> _repository;
    private readonly IMapper _mapper;

    public OrderStateService(IGenericRepository<OrderStatus> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<OrderStatusDto> AddOrderStateAsync(OrderStatusDto orderStateDto)
    {
        var entity = _mapper.Map<OrderStatus>(orderStateDto);
        await _repository.AddAsync(entity);
        await _repository.SaveAsync();
        return _mapper.Map<OrderStatusDto>(entity);
    }

    public async Task<OrderStatusDto> UpdateOrderStateAsync(OrderStatusDto orderStateDto)
    {
        var entity = _mapper.Map<OrderStatus>(orderStateDto);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<OrderStatusDto>(entity);
    }

    public async Task DeleteOrderStateAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity != null)
        {
            await _repository.DeleteAsync(entity);
        }
    }

    public async Task<OrderStatusDto> GetOrderStateByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map<OrderStatusDto>(entity);
    }

    public async Task<IEnumerable<OrderStatusDto>> GetAllOrderStatesAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderStatusDto>>(entities);
    }
}