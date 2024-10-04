using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

public class PaymentService : IPaymentService
{
    private readonly IGenericRepository<Payment> _paymentRepository;

    public PaymentService(IGenericRepository<Payment> paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    

    public async Task<Payment> GetDefaultPaymentAsync()
    {
        var defaultPayment = await _paymentRepository.GetByIdAsync(1); // Assuming 1 is the default payment ID
        if (defaultPayment == null)
        {
            defaultPayment = new Payment { Method = "Default Payment Method" };
            await _paymentRepository.AddAsync(defaultPayment);
            await _paymentRepository.SaveAsync();
        }
        return defaultPayment;
    }
}