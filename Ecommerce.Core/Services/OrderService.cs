using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OrderService : IOrderService
{
    private readonly IGenericRepository<Order> _orderRepository;
    private readonly IGenericRepository<OrderStatus> _orderStatusRepository;
    private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICartService _cartService;
    private readonly IShippingService _shippingService;
    private readonly IShipperService _shipperService;
    private readonly IDiscountService _discountService;
    private readonly IMapper _mapper;

    public OrderService(
        IGenericRepository<Order> orderRepository,
        IGenericRepository<OrderStatus> orderStatusRepository,
        IGenericRepository<OrderDetail> orderDetailRepository,
        IProductRepository productRepository,
        ICartService cartService,
        IShippingService shippingService,
        IShipperService shipperService,
        IDiscountService discountService,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderStatusRepository = orderStatusRepository;
        _orderDetailRepository = orderDetailRepository;
        _productRepository = productRepository;
        _cartService = cartService;
        _shippingService = shippingService;
        _shipperService = shipperService;
        _discountService = discountService;
        _mapper = mapper;
    }

    public async Task<OrderDto> CreateOrderAsync(int userId)
    {
        var cart = await _cartService.GetCartByUserIdAsync(userId);
        var orderDetails = new List<OrderDetail>();

        foreach (var cartItem in cart.CartItems)
        {
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            var discount = await _discountService.FindAsync(d => d.ProductId == cartItem.ProductId && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now);
            var discountAmount = discount?.FirstOrDefault()?.DiscountAmount ?? 0;

            var orderDetail = new OrderDetail
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = product.UnitPrice,
                DiscountAmount = discountAmount,
                Price = (product.UnitPrice - discountAmount) * cartItem.Quantity
            };
            orderDetails.Add(orderDetail);
        }

        var order = new Order
        {
            OrderStatusId = 1, // Assuming 1 is the default status
            PaymentId = 0, // Set appropriate payment ID
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            ShippingCost = 0, // Set appropriate shipping cost
            TrackingNumber = string.Empty,
            ShippingMethodId = 0, // Set appropriate shipping method ID
            OrderDetails = orderDetails
        };

        order.TotalAmount = order.CalculateTotal();

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveAsync();

        // Create shipping details
        var shipper = await _shipperService.AssignOrderToLeastAssignedShipper(); // Assuming a method to get default shipper
        var trackingCode = Guid.NewGuid().ToString();

        var shippingDto = new ShippingDto
        {
            OrderId = order.Id,
            ShipperId = shipper.Id,
            ShippingMethod = ShippingMethod.Standard, // Set appropriate shipping method
            Price = 10, // Set appropriate shipping cost
            TrackingNumber = trackingCode,
            ShippingStatus = ShippingStatus.Pending
        };
        await _shippingService.CreateShippingAsync(shippingDto);

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task UpdateOrderAsync(OrderDto orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        await _orderRepository.UpdateAsync(order);
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order != null)
        {
            await _orderRepository.DeleteAsync(order);
        }
    }
  

    public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderState status)
    {
        var orders = await _orderRepository.GetAllAsync(o => o.OrderStatus != null && o.OrderStatus.Status == status);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task ChangeOrderStatusAsync(int orderId, OrderState newStatus)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order != null)
        {
            if (order.OrderStatus != null)
            {
                order.OrderStatus.Status = newStatus;
            }
            order.UpdatedAt = DateTime.Now;
            await _orderRepository.UpdateAsync(order);
        }
    }
}
