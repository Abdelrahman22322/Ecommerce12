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
    private readonly IGenericRepository<ShippingState> _shippingStateRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICartService _cartService;
    private readonly IShippingService _shippingService;
    private readonly IShipperService _shipperService;
    private readonly IDiscountService _discountService;
    private readonly IMapper _mapper;
    private readonly ICheckoutService _checkoutService;
    private readonly IPaymentService _paymentService;
    private readonly IShippingMethodService _shippingMethodService;
    private readonly IShippingStateService _shippingStateService;
    

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
        IPaymentService paymentService,
        IShippingMethodService shippingMethodService,
        IShippingStateService shippingStateService, IGenericRepository<ShippingState> shippingStateRepository)
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
        _shippingMethodService = shippingMethodService;
        _shippingStateService = shippingStateService;
        _shippingStateRepository = shippingStateRepository;
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
    }


    public async Task<OrderDto> CreateOrderAsync(OrderCreateDto orderCreateDto)
    {
        try
        {
            var userProfile = await _checkoutService.GetUserProfileByIdAsync(orderCreateDto.UserId)
                              ?? throw new KeyNotFoundException("User profile not found.");

            var cart = await _cartService.GetCartByUserIdAsync(orderCreateDto.UserId)
                       ?? throw new InvalidOperationException("Cart is empty or not found.");

            var orderDetails = await PrepareOrderDetails(cart);

            var shippingMethod = await _shippingMethodService.GetShippingMethodByIdAsync(orderCreateDto.ShippingMethodId)
                                ?? throw new KeyNotFoundException("Shipping method not found.");

            var shippingPrice = shippingMethod.Cost;
            var totalAmount = orderDetails.Sum(od => od.Price) + shippingPrice;

            var shipper = await _shipperService.AssignOrderToLeastAssignedShipper();
            if (shipper == null)
            {
                throw new Exception("No shippers available.");
            }

            // Create and save the ShippingState entity
            var shippingState = new ShippingState { Status = ShippingStatus.Pending };
            await _shippingStateRepository.AddAsync(shippingState);
            await _shippingStateRepository.SaveAsync();

            // Generate a new GUID for the tracking number
            var trackingNumber = Guid.NewGuid().ToString();

            var shipping = new Shipping
            {
                ShippingMethodId = shippingMethod.Id,
                ShippingCost = shippingPrice,
                ShippingStateId = shippingState.Id, // Set the foreign key
                TrackingNumber = trackingNumber,
                ShipperId = shipper.Id
            };

            var shippingDto = _mapper.Map<ShippingDto>(shipping);
            var createdShipping = await _shippingService.CreateShippingAsync(shippingDto);
            shipping.Id = createdShipping.Id;

            var order = new Order
            {
                UserId = orderCreateDto.UserId,
                OrderStatusId = (await _orderStatusRepository.GetAllAsync()).FirstOrDefault()?.Id ?? 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ShippingCost = shippingPrice,
                OrderDetails = orderDetails,
                TotalAmount = totalAmount,
                PaymentStatus = "pending",
                ShippingId = shipping.Id,
                PaymentId = (await _paymentService.GetDefaultPaymentAsync()).Id,
                TrackingNumber = trackingNumber // Set the TrackingNumber
            };

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveAsync();

            var checkoutUrl = await CreateStripeCheckoutSessionAsync(orderDetails, totalAmount, shippingPrice, order.Id);

            return new OrderDto
            {
                CheckoutUrl = checkoutUrl,
                TotalAmount = totalAmount,
                Id = order.Id
            };
        }
        catch (Exception ex)
        {
            // Log the exception details
            //_logger.LogError(ex, "An error occurred while creating the order.");
            //if (ex.InnerException != null)
            //{
            //    _logger.LogError(ex.InnerException, "Inner exception details.");
            //}
            throw new Exception("An error occurred while creating the order. Please try again later.", ex.InnerException);
        }
    }


    private async Task<string> CreateStripeCheckoutSessionAsync(List<OrderDetail> orderDetails, decimal totalAmount, decimal shippingCost, int orderId)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
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

            // Add shipping cost as a separate line item
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Shipping Cost",
                    },
                    UnitAmount = (long)(shippingCost * 100), // Stripe expects the amount in cents
                },
                Quantity = 1,
            });

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session.Url;
        }
        catch (StripeException ex)
        {
            // Log the exception details
            //_logger.LogError(ex, "Stripe error occurred: {Message}", ex.Message);
            throw new Exception("An error occurred while creating the Stripe checkout session. Please try again later.", ex);
        }
    }


    public async Task<string> CompleteOrderAsync(int orderId)
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

        var shippingPrice = await _shippingMethodService.GetShippingPriceAsync(order.Shipping.ShippingMethodId);
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

        return shipping.TrackingNumber;
    }


    private OrderCreateDto GetOrderCreateDtoFromCharge(Charge charge)
    {
        // Implement this method to extract order details from the charge
        // This is a placeholder implementation
        return new OrderCreateDto
        {
            UserId = int.Parse(charge.Metadata["user_id"]),
            ShippingMethodId = int.Parse(charge.Metadata["shipping_method_id"]),
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
     //   shipping.Method = (ShippingMethod)orderCreateDto.ShippingMethodId;
        shipping.ShippingMethod.Cost = shippingPrice;
        shipping.ShippingState = new ShippingState { Status = ShippingStatus.Pending };
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
            d.Products.Any(x => x.ProductId == productId));

        return discount?.FirstOrDefault()?.DiscountAmount ?? 0;
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
