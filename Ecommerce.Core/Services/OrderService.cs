using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.Enums;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Guid = System.Guid;

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
    private readonly ICheckoutService _checkoutService;
    private readonly IPaymentService _paymentService;

    public OrderService(
        IGenericRepository<Order> orderRepository,
        IGenericRepository<OrderStatus> orderStatusRepository,
        IGenericRepository<OrderDetail> orderDetailRepository,
        IProductRepository productRepository,
        ICartService cartService,
        IShippingService shippingService,
        IShipperService shipperService,
        IDiscountService discountService,
        IMapper mapper, ICheckoutService checkoutService, IConfiguration configuration, IPaymentService paymentService)
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
        _checkoutService = checkoutService;
        _paymentService = paymentService;
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
    }

    private async Task<string> CreateStripeCheckoutSessionAsync(Order order)
    {
        if (order.OrderDetails == null)
        {
            throw new ArgumentNullException(nameof(order.OrderDetails), "Order details cannot be null.");
        }

        var options = new SessionCreateOptions
        {
            LineItems = order.OrderDetails.Select(od => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(od.Price * 100), // Stripe expects the amount in cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = od.OrderId.ToString(),
                    },
                },
                Quantity = od.Quantity,
            }).ToList(),
            Mode = "payment",
            SuccessUrl = "http://localhost:5190/success",
            CancelUrl = "http://localhost:5190/cancel",
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session.Url;
    }

    private async Task<decimal> GetPriceAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        return product.UnitPrice;
    }




    //public async Task<OrderDto> CreateOrderAsync(OrderCreateDto orderCreateDto)
    //{
    //    // Fetch user profile data
    //    var userProfile = await _checkoutService.GetUserProfileByIdAsync(orderCreateDto.UserId);
    //    if (userProfile == null)
    //    {
    //        throw new KeyNotFoundException("User profile not found.");
    //    }

    //    var cart = await _cartService.GetCartByUserIdAsync(orderCreateDto.UserId);
    //    var orderDetails = new List<OrderDetail>();

    //    foreach (var cartItem in cart.CartItems)
    //    {
    //        if (cartItem == null) continue; // Ensure cartItem is not null

    //        var price = await GetPriceAsync(cartItem.ProductId);
    //        var discount = await _discountService.FindAsync(d =>
    //            d.ProductId == cartItem.ProductId && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now);
    //        var discountAmount = discount?.FirstOrDefault()?.DiscountAmount ?? 0;

    //        var product = await _productRepository.GetByIdAsync(cartItem.ProductId); // Fetch the product

    //        var orderDetail = _mapper.Map<OrderDetail>(cartItem); // Use AutoMapper
    //      //  orderDetail.Product = product; // Assign the product
    //        orderDetail.UnitPrice = price;
    //        orderDetail.DiscountAmount = discountAmount;
    //        orderDetail.Price = (price - discountAmount) * cartItem.Quantity;

    //        orderDetails.Add(orderDetail);
    //    }

    //    var shippingPrice = _shippingService.GetShippingPrice(orderCreateDto.ShippingMethod);

    //    // Ensure the default order status exists
    //    var defaultOrderStatus = await _orderStatusRepository.GetByIdAsync((int)OrderState.Pending); // Assuming Pending is the default status
    //    if (defaultOrderStatus == null)
    //    {
    //        defaultOrderStatus = new OrderStatus { Status = OrderState.Pending };
    //        await _orderStatusRepository.AddAsync(defaultOrderStatus);
    //        await _orderStatusRepository.SaveAsync();
    //    }

    //    // Ensure the default payment method exists
    //    var defaultPayment = await _paymentService.GetDefaultPaymentAsync(); // Use PaymentService

    //    // Ensure the chosen shipping method is valid
    //    if (!await _shippingService.IsValidShippingMethodAsync(orderCreateDto.ShippingMethod))
    //    {
    //        throw new ArgumentException("Invalid shipping method.");
    //    }

    //    // Create the shipping entry first
    //    var shipper = await _shipperService.AssignOrderToLeastAssignedShipper(); // Assuming a method to get default shipper
    //    var trackingCode = Guid.NewGuid().ToString();

    //    var shippingDto = _mapper.Map<ShippingDto>(userProfile);
    //    shippingDto.ShippingMethod = orderCreateDto.ShippingMethod;
    //    shippingDto.Cost = shippingPrice;
    //    shippingDto.TrackingCode = trackingCode;
    //    shippingDto.ShippingStatus = ShippingStatus.Pending;
    //    shippingDto.Price = shippingPrice;
    //    shippingDto.TrackingNumber = trackingCode;
    //    shippingDto.ShipperId = shipper.Id;

    //    var createdShipping = await _shippingService.CreateShippingAsync(shippingDto);

    //    var order = _mapper.Map<Order>(orderCreateDto); // Use AutoMapper
    //    order.OrderStatusId = defaultOrderStatus.Id; // Use the default order status ID
    //    order.PaymentId = defaultPayment.Id; // Use the default payment ID
    //    order.CreatedAt = DateTime.Now;
    //    order.UpdatedAt = DateTime.Now;
    //    order.ShippingCost = shippingPrice;
    //    order.TrackingNumber = trackingCode;
    //   // order.ShippingMethodId = createdShipping.Id; // Set the created shipping ID
    //    order.OrderDetails = orderDetails;

    //    order.TotalAmount = order.CalculateTotal() + shippingPrice;

    //    await _orderRepository.AddAsync(order);
    //    await _orderRepository.SaveAsync();

    //    // Update the shipping entry with the correct order ID
    //    createdShipping.OrderId = order.Id;
    //  //  await _shippingService.UpdateShippingStatusAsync(createdShipping.Id, createdShipping.ShippingStatus);

    //    var checkoutUrl = await CreateStripeCheckoutSessionAsync(order);

    //    var orderDto = _mapper.Map<OrderDto>(order);
    //    orderDto.CheckoutUrl = checkoutUrl; // Add checkout URL to the DTO

    //    return orderDto;
    //}
    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto orderCreateDto)
    {
      
        var userProfile = await _checkoutService.GetUserProfileByIdAsync(orderCreateDto.UserId)
                        ?? throw new KeyNotFoundException("User profile not found.");

      
        var cart = await _cartService.GetCartByUserIdAsync(orderCreateDto.UserId)
                        ?? throw new InvalidOperationException("Cart is empty or not found.");

      
        var orderDetails = await PrepareOrderDetails(cart);

       
        var shippingPrice = _shippingService.GetShippingPrice(orderCreateDto.ShippingMethod);

      
        var defaultOrderStatus = await EnsureDefaultOrderStatusAsync();

      
        if (!await _shippingService.IsValidShippingMethodAsync(orderCreateDto.ShippingMethod))
            throw new ArgumentException("Invalid shipping method.");

      
        var shippingDto = await PrepareShipping(userProfile, orderCreateDto, shippingPrice);
        var createdShipping = await _shippingService.CreateShippingAsync(shippingDto);


        var order = await CreateOrder(orderCreateDto, defaultOrderStatus.Id, shippingPrice, orderDetails);

        
        order.ShippingId = createdShipping.Id;

       
        order.TotalAmount = order.CalculateTotal() + shippingPrice;

      
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveAsync();

        
        createdShipping.OrderId = order.Id;

       
        var checkoutUrl = await CreateStripeCheckoutSessionAsync(order);

        
        var orderDto = _mapper.Map<OrderDto>(order);
        orderDto.CheckoutUrl = checkoutUrl;

        return orderDto;
    }


    private async Task<List<OrderDetail>> PrepareOrderDetails(Cart cart)
    {
        var orderDetails = new List<OrderDetail>();

        foreach (var cartItem in cart.CartItems)
        {
            if (cartItem == null) continue;

            var price = await GetPriceAsync(cartItem.ProductId);
            var discountAmount = await GetDiscountAmountAsync(cartItem.ProductId);

            var orderDetail = _mapper.Map<OrderDetail>(cartItem);
            orderDetail.UnitPrice = price;
            orderDetail.DiscountAmount = discountAmount;
            orderDetail.Price = (price - discountAmount) * cartItem.Quantity;

            orderDetails.Add(orderDetail);
        }

        return orderDetails;
    }

    private async Task<decimal> GetDiscountAmountAsync(int productId)
    {
        var discount = await _discountService.FindAsync(d =>
            d.ProductId == productId && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now);

        return discount?.FirstOrDefault()?.DiscountAmount ?? 0;
    }

    private async Task<OrderStatus> EnsureDefaultOrderStatusAsync()
    {
        var defaultOrderStatus = await _orderStatusRepository.GetByIdAsync((int)OrderState.Pending);

        if (defaultOrderStatus == null)
        {
            defaultOrderStatus = new OrderStatus { Status = OrderState.Pending };
            await _orderStatusRepository.AddAsync(defaultOrderStatus);
            await _orderStatusRepository.SaveAsync();
        }

        return defaultOrderStatus;
    }

    private async Task<Order> CreateOrder(OrderCreateDto orderCreateDto, int orderStatusId, decimal shippingCost,  List<OrderDetail> orderDetails)
    {
        var order = _mapper.Map<Order>(orderCreateDto);
        order.OrderStatusId = orderStatusId;
        order.PaymentId = (await _paymentService.GetDefaultPaymentAsync()).Id;
        order.CreatedAt = DateTime.Now;
        order.UpdatedAt = DateTime.Now;
        order.ShippingCost = shippingCost;
      //  order.ShippingId = shippingId;
        order.OrderDetails = orderDetails;

        return order;
    }
    private async Task<ShippingDto> PrepareShipping(UserProfile userProfile, OrderCreateDto orderCreateDto, decimal shippingPrice)
    {
        var shipper = await _shipperService.AssignOrderToLeastAssignedShipper();
        var trackingCode = Guid.NewGuid().ToString();

        var shippingDto = _mapper.Map<ShippingDto>(userProfile);
        shippingDto.ShippingMethod = orderCreateDto.ShippingMethod;
        shippingDto.Cost = shippingPrice;
    //    shippingDto.TrackingCode = trackingCode = new Guid().ToString();
        shippingDto.ShippingStatus = ShippingStatus.Pending;
        shippingDto.Price = shippingPrice;
        shippingDto.TrackingNumber = trackingCode ;
        shippingDto.ShipperId = shipper.Id;

        return shippingDto;
    }


    public async Task UpdateOrderStatusAsync(int orderId, OrderState newStatus)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order != null)
        {
            order.OrderStatusId = (int)newStatus;
            order.UpdatedAt = DateTime.Now;
            await _orderRepository.UpdateAsync(order);
        }
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
