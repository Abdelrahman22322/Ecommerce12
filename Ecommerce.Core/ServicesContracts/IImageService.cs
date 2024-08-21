using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.ServicesContracts;

public interface IImageService
{
    Task<CloudinaryResult> UploadImageAsync(IFormFile uploadParams);
    Task DeleteImageAsync(string publicId);
    Task AddImageAsync(ProductImage? image);
    Task<ProductImage?> GetImageByIdAsync(int id);
     Task<IEnumerable<ImageDto>> GetImageByProductIdAsync(int id);
}