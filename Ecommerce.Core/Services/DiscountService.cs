using System.Linq.Expressions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IGenericRepository<Discount> _discountRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public DiscountService(
            IGenericRepository<Discount> discountRepository,
            IGenericRepository<Product> productRepository)
        {
            _discountRepository = discountRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Discount?>> GetAllAsync()
        {
            var discounts = await _discountRepository.GetAllAsync(null, null);
            return discounts.ToList();
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
                ProductIds = discount.Products.Select(p => p.ProductId).ToList()
            };
        }

        public async Task AddAsync(DiscountDTO discount)
        {
            ValidateDiscount(discount);

            var products = await _productRepository.FindAsync(p => discount.ProductIds != null && discount.ProductIds.Contains(p.ProductId), null);
            if (!products.Any())
            {
                throw new InvalidOperationException("One or more products do not exist.");
            }

            var newDiscount = new Discount
            {
                DiscountAmount = discount.DiscountAmount,
                DiscountName = discount.DiscountName,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                Products = new List<Product>()
            };

            foreach (var product in products)
            {
                // Attach the existing product to the context
                _productRepository.Attach(product);
                newDiscount.Products.Add(product);
            }

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

            var products = await _productRepository.FindAsync(p => discount.ProductIds != null && discount.ProductIds.Contains(p.ProductId), null);
            if (!products.Any())
            {
                throw new InvalidOperationException("One or more products do not exist.");
            }

            existingDiscount.DiscountAmount = discount.DiscountAmount;
            existingDiscount.StartDate = discount.StartDate;
            existingDiscount.EndDate = discount.EndDate;
            existingDiscount.Products.Clear();

            foreach (var product in products)
            {
                // Attach the existing product to the context
                _productRepository.Attach(product);
                existingDiscount.Products.Add(product);
            }

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

        public async Task<IEnumerable<Discount?>> FindAsync(Expression<Func<Discount?, bool>> func)
        {
            return await _discountRepository.FindAsync(func, null);
        }

        public async Task RemoveProductFromDiscountAsync(int discountId, int productId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null)
            {
                throw new Exception("Discount not found.");
            }

            var productToRemove = discount.Products.FirstOrDefault(p => p.ProductId == productId);
            if (productToRemove != null)
            {
                discount.Products.Remove(productToRemove);
                await _discountRepository.UpdateAsync(discount);
                await _discountRepository.SaveAsync();
            }
            else
            {
                throw new Exception("Product not found in discount.");
            }
        }

        private void ValidateDiscount(DiscountDTO discount)
        {
            if (discount == null)
            {
                throw new ArgumentNullException(nameof(discount), "Discount cannot be null.");
            }

            if (discount.DiscountAmount <= 0)
            {
                throw new ArgumentException("Discount amount must be greater than zero.", nameof(discount.DiscountAmount));
            }

            if (discount.StartDate >= discount.EndDate)
            {
                throw new ArgumentException("Start date must be earlier than end date.", nameof(discount.StartDate));
            }

            if (discount.ProductIds == null || !discount.ProductIds.Any())
            {
                throw new ArgumentException("At least one product ID must be provided.", nameof(discount.ProductIds));
            }

            foreach (var productId in discount.ProductIds)
            {
                if (productId <= 0)
                {
                    throw new ArgumentException($"Invalid product ID: {productId}", nameof(discount.ProductIds));
                }
            }
        }
    }
}
