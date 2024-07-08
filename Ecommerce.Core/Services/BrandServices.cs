using Ecommerce.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.ServicesContracts;

namespace Ecommerce.Core.Services
{    
   

    
public class BrandServices : IBrandServices
    {
        private readonly IGenericRepository<Brand> _brandRepository;

        public BrandServices(IGenericRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task AddBrand(Brand entity)
        {
            if (entity.Name == null)
            {
                throw new ArgumentNullException("band name required");
            }

            await _brandRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(Brand entity)
        {

            if (entity.Name == null)
            {
                throw new ArgumentNullException("band name required");
            }
            await _brandRepository.UpdateAsync(entity);
            await _brandRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            var brand = await _brandRepository.GetByIdAsync(id);
            await _brandRepository.DeleteAsync(brand);
            await _brandRepository.SaveAsync();

            return true;

        }

        public async Task<bool> DeleteRange(IEnumerable<Brand> entities)
        {

            await _brandRepository.DeleteRange(entities);
            await _brandRepository.SaveAsync();
            return true;
        }

        public async Task<Brand> GetByIdAsync(int id)
        {

            return await _brandRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>>? predicate, string? includeword)
        {

            return await _brandRepository.GetAllAsync(predicate, includeword : "product");
        }

        public async Task SaveAsync()
        {
            await _brandRepository.SaveAsync();
        }
    }
}