using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services;

public class DiscountService : IDiscountService
{
    private readonly IGenericRepository<Discount> _discountRepository;

    public DiscountService(IGenericRepository<Discount> discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<IEnumerable<Discount?>> GetAllAsync()
    {
        var discounts = await _discountRepository.GetAllAsync(null, null);
           
        return discounts;

    }

    public async Task<Discount> GetByIdAsync(int id)
    {
        var discount = await _discountRepository.GetByIdAsync(id);
        if (discount == null)
        {
            throw new Exception("Discount not found.");
        }

        return discount;
    }

    public async Task AddAsync(Discount? discount)
    {
        

        await _discountRepository.AddAsync(discount);
       
    }



    public async Task UpdateAsync(int id, Discount discount)
    {
        var existingDiscount = await _discountRepository.GetByIdAsync(id);
        if (existingDiscount == null)
        {
            throw new Exception("Discount not found.");
        }

        // Ensure the ID in the URL matches the ID in the Discount object
        if (id != discount.DiscountId)
        {
            throw new ArgumentException("ID mismatch.");
        }

        // Manually update properties
        existingDiscount.DiscountAmount= discount.DiscountAmount;
     
        existingDiscount.StartDate = discount.StartDate;
        existingDiscount.EndDate = discount.EndDate;

        await _discountRepository.UpdateAsync(existingDiscount);
        await _discountRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {

        var discount = await _discountRepository.GetByIdAsync(id);
        if (discount == null)
        {
            throw new Exception("Discount not found.");
        }

        await _discountRepository.DeleteAsync(discount);
        await _discountRepository.SaveAsync();

    }

    public async Task<IEnumerable<Discount?>> FindAsync(Expression<Func<Discount, bool>?> func)
    {

        return await _discountRepository.FindAsync(func);
    }
}