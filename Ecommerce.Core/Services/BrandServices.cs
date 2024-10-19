using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services
{
    public class BrandServices : IBrandServices
    {
        private readonly IGenericRepository<Brand?> _brandRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public BrandServices(IGenericRepository<Brand?> brandRepository, IImageService imageService, IMapper mapper)
        {
            _brandRepository = brandRepository ?? throw new ArgumentNullException(nameof(brandRepository));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddBrand(CreateBrandDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentNullException("Brand name is required");
            }

            var brand = _mapper.Map<Brand>(dto);

            if (dto.ImageFile != null)
            {
                var uploadResult = await _imageService.UploadImageAsync(dto.ImageFile);
                brand.ImageUrl = uploadResult.Url;
            }

            await _brandRepository.AddAsync(brand);
            await _brandRepository.SaveAsync();
        }

        public async Task<bool> UpdateAsync(UpdateBrandDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentNullException("Brand name is required");
            }

            var brand = await _brandRepository.GetByIdAsync(dto.Id);
            if (brand == null)
            {
                throw new KeyNotFoundException("Brand not found");
            }

            _mapper.Map(dto, brand);

            if (dto.ImageFile != null)
            {
                var uploadResult = await _imageService.UploadImageAsync(dto.ImageFile);
                brand.ImageUrl = uploadResult.Url;
            }

            await _brandRepository.UpdateAsync(brand);
            await _brandRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                throw new KeyNotFoundException("Brand not found");
            }

            await _brandRepository.DeleteAsync(brand);
            await _brandRepository.SaveAsync();
            return true;
        }

        public async Task<BrandDto> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                throw new KeyNotFoundException("Brand not found");
            }

            return _mapper.Map<BrandDto>(brand);
        }

        //public async Task<IEnumerable<BrandDto>> GetAllAsync(Expression<Func<Brand, bool>>? predicate = null)
        //{
        //    var brands = predicate == null
        //        ? await _brandRepository.GetAllAsync()
        //        : await _brandRepository.FindAsync(predicate);

        //    return _mapper.Map<IEnumerable<BrandDto>>(brands);
        //}

        public async Task<Brand?> DetermineBrandAsync(string brandName)
        {
            var existingBrands = await _brandRepository.FindAsync(x => x.Name == brandName, null);
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

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {

            var brands = await _brandRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDto>>(brands);
        }
    }
}
