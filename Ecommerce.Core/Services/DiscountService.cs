using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IGenericRepository<Discount> _discountRepository;
        private readonly IGenericRepository<Product> _productRepository; // Add a repository for products

        public DiscountService(IGenericRepository<Discount> discountRepository, IGenericRepository<Product> productRepository)
        {
            _discountRepository = discountRepository;
            _productRepository = productRepository; // Initialize the product repository
        }

        public async Task<IEnumerable<Discount>> GetAllAsync()
        {
            var discounts = await _discountRepository.GetAllAsync(null, null);

            IEnumerable<Discount> allAsync = discounts.ToList();
            if (!allAsync.Any())
            {
                return [];
            }

            return allAsync;
            //return discounts.Select(discount => new DiscountDTO
            //{
            //    DiscountAmount = discount.DiscountAmount,
            //    StartDate = discount.StartDate,
            //    EndDate = discount.EndDate
            //}).ToList();
        }

        public async Task<DiscountDTO> GetByIdAsync(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            if (discount == null)
            {
                throw new Exception("Discount not found.");
            }

            return new DiscountDTO()
            {

                DiscountAmount = discount.DiscountAmount,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                ProductId = discount.ProductId

            };

        }

        public async Task AddAsync(DiscountDTO discount)
        {
            ValidateDiscount(discount);

            // Check if the product exists
            var product = await _productRepository.GetByIdAsync(discount.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException("Product does not exist.");
            }

            // Validate if the product already has a discount
            var existingDiscounts = await _discountRepository.FindAsync(d => d.ProductId == discount.ProductId);
            if (existingDiscounts.Any())
            {
                throw new InvalidOperationException("A discount already exists for this product.");
            }

            var newDiscount = new Discount
            {
                DiscountAmount = discount.DiscountAmount,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                ProductId = discount.ProductId
            };

            await _discountRepository.AddAsync(newDiscount);
            await _discountRepository.SaveAsync();
        }

        public async Task UpdateAsync(int id, DiscountDTO discount)
        {
            ValidateDiscount(discount);

            var existingDiscount = await _discountRepository.GetByIdAsync(id);
            if (existingDiscount == null)
            {
                throw new Exception("Discount not found.");
            }

            // Manually update properties
            existingDiscount.DiscountAmount = discount.DiscountAmount;
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

        public async Task<IEnumerable<Discount?>> FindAsync(Expression<Func<Discount, bool>> func)
        {
            return await _discountRepository.FindAsync(func);
        }

        private void ValidateDiscount(DiscountDTO discount)
        {
            if (discount.DiscountAmount <= 0)
            {
                throw new ArgumentException("Discount amount must be greater than zero.");
            }

            if (discount.StartDate >= discount.EndDate)
            {
                throw new ArgumentException("Start date must be earlier than end date.");
            }

            if (discount.ProductId <= 0)
            {
                throw new ArgumentException("Invalid product ID.");
            }
        }
    }
}
