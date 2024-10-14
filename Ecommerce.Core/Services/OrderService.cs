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
using Shipping = Ecommerce.Core.Domain.Entities.Shipping;

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
        IMapper mapper,
        ICheckoutService checkoutService,
        IConfiguration configuration,
        IPaymentService paymentService)
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


    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto orderCreateDto)
    {
        var userProfile = await _checkoutService.GetUserProfileByIdAsync(orderCreateDto.UserId)
                          ?? throw new KeyNotFoundException("User profile not found.");

        var cart = await _cartService.GetCartByUserIdAsync(orderCreateDto.UserId)
                   ?? throw new InvalidOperationException("Cart is empty or not found.");

        var orderDetails = await PrepareOrderDetails(cart);

        var shippingPrice = _shippingService.GetShippingPrice(orderCreateDto.ShippingMethod);
        var totalAmount = orderDetails.Sum(od => od.Price) + shippingPrice;

        // Fetch or create a Shipper record
        var shipper = await _shipperService.AssignOrderToLeastAssignedShipper();
        if (shipper == null)
        {
            throw new Exception("No shippers available.");
        }

        // Create a Shipping record
        var shipping = new Shipping
        {
            Method = orderCreateDto.ShippingMethod,
            Price = shippingPrice,
            Status = ShippingStatus.Pending,
            TrackingNumber = Guid.NewGuid().ToString(),
            ShipperId = shipper.Id // Ensure ShipperId is set correctly
        };

        // Save the Shipping record and get its Id
        var shippingDto = _mapper.Map<ShippingDto>(shipping);
        var createdShipping = await _shippingService.CreateShippingAsync(shippingDto);
        shipping.Id = createdShipping.Id; // Ensure the Id is set after creation

        // Create a pending order
        var order = new Order
        {
            UserId = orderCreateDto.UserId,
            OrderStatusId = (await _orderStatusRepository.GetAllAsync()).FirstOrDefault()?.Id ?? 0,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            ShippingCost = shippingPrice,
            OrderDetails = orderDetails,
            TotalAmount = totalAmount,
            PaymentStatus = "pending", // Set initial payment status to pending
            ShippingId = shipping.Id, // Ensure ShippingId is set correctly
            PaymentId = (await _paymentService.GetDefaultPaymentAsync()).Id
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveAsync();

        var checkoutUrl = await CreateStripeCheckoutSessionAsync(orderDetails, totalAmount, order.Id);

        return new OrderDto
        {
            CheckoutUrl = checkoutUrl,
            TotalAmount = totalAmount,
            Id = order.Id
        };
    }





    private async Task<string> CreateStripeCheckoutSessionAsync(List<OrderDetail> orderDetails, decimal totalAmount, int orderId)
    {
        var options = new SessionCreateOptions
        {
            LineItems = orderDetails.Select(od => new SessionLineItemOptions
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
            SuccessUrl = $"http://localhost:5190/success?orderId={orderId}",
            CancelUrl = "http://localhost:5190/cancel",
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        return session.Url;
    }

    public async Task CompleteOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId)
                    ?? throw new KeyNotFoundException("Order not found.");

        if (order.PaymentStatus != "succeeded")
        {
            throw new InvalidOperationException("Payment not completed.");
        }

        var userProfile = await _checkoutService.GetUserProfileByIdAsync(order.UserId)
                          ?? throw new KeyNotFoundException("User profile not found.");

        var cart = await _cartService.GetCartByUserIdAsync(order.UserId)
                   ?? throw new InvalidOperationException("Cart is empty or not found.");

        var orderDetails = await PrepareOrderDetails(cart);

        var shippingPrice = _shippingService.GetShippingPrice(order.Shipping.Method);
        var orderStatusId = (await _orderStatusRepository.GetAllAsync()).FirstOrDefault()?.Id ?? 0;

        order.OrderStatusId = orderStatusId;
        order.UpdatedAt = DateTime.Now;
        order.ShippingCost = shippingPrice;
        order.OrderDetails = orderDetails;

        var orderCreateDto = _mapper.Map<OrderCreateDto>(order);
        var shipping = await CreateShippingAsync(userProfile, orderCreateDto, shippingPrice);

        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveAsync();

        await _shippingService.CreateShippingAsync(_mapper.Map<ShippingDto>(shipping));
    }

    private OrderCreateDto GetOrderCreateDtoFromCharge(Charge charge)
    {
        // Implement this method to extract order details from the charge
        // This is a placeholder implementation
        return new OrderCreateDto
        {
            UserId = int.Parse(charge.Metadata["user_id"]),
            ShippingMethod = ShippingMethod.Standard,
            // Add other necessary properties
        };
    }



    private async Task<Order> CreateOrder(OrderCreateDto orderCreateDto, int orderStatusId, decimal shippingCost, List<OrderDetail> orderDetails)
    {
        var order = _mapper.Map<Order>(orderCreateDto);
        order.OrderStatusId = orderStatusId;
        order.PaymentId = (await _paymentService.GetDefaultPaymentAsync()).Id;
        order.CreatedAt = DateTime.Now;
        order.UpdatedAt = DateTime.Now;
        order.ShippingCost = shippingCost;
        order.OrderDetails = orderDetails;

        return order;
    }

    private async Task<Shipping> CreateShippingAsync(UserProfile userProfile, OrderCreateDto orderCreateDto, decimal shippingPrice)
    {
        var shipper = await _shipperService.AssignOrderToLeastAssignedShipper();
        var trackingCode = Guid.NewGuid().ToString();

        var shipping = _mapper.Map<Shipping>(userProfile);
        shipping.Method = orderCreateDto.ShippingMethod;
        shipping.Price = shippingPrice;
        shipping.Status = ShippingStatus.Pending;
        shipping.Price = shippingPrice;
        shipping.TrackingNumber = trackingCode;
        shipping.ShipperId = shipper.Id;

        return shipping;
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
            d.Products.Any(x=> x.ProductId == productId));

        return discount?.FirstOrDefault()?.DiscountAmount ?? 0;
    }


    private async Task<string> CreateStripeCheckoutSessionAsync(List<OrderDetail> orderDetails, decimal totalAmount)
    {
        var options = new SessionCreateOptions
        {
            LineItems = orderDetails.Select(od => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(od.SubTotal * 100), // Stripe expects the amount in cents (total amount in cents)
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

   

    private async Task<decimal> GetPriceAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        return product.UnitPrice;
    }

   
}
