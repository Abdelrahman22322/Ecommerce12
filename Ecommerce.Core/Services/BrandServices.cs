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
        private readonly IGenericRepository<Brand?> _brandRepository;

        public BrandServices(IGenericRepository<Brand?> brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task AddBrand(Brand? entity)
        {
            if (entity.Name == null)
            {
                throw new ArgumentNullException("band name required");
            }

            await _brandRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(Brand? entity)
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
            throw new NotImplementedException();
        }

        public async Task<Brand> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>>? predicate, string? includeword)
        {
            throw new NotImplementedException();
        }


        public async Task<Brand?> DetermineBrandAsync(string brandName)
        {
            var existingBrands = await _brandRepository.FindAsync(x => x.Name == brandName);
            var existingBrand = existingBrands?.FirstOrDefault();
            if (existingBrand == null)
            {
                var brand = new Brand
                {
                    Name = brandName
                };
                await _brandRepository.AddAsync(brand);
                await _brandRepository.SaveAsync();
                return brand;
            }

            return existingBrand;
        }


    }
    

}