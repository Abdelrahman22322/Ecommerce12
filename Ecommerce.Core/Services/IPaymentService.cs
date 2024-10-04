using Ecommerce.Core.Domain.Entities;

public interface IPaymentService
{
    Task<Payment> GetDefaultPaymentAsync();
}