using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public interface IImageRepository
{
    Task<CloudinaryResult> UploadImageAsync( IFormFile uploadParams);
    Task DeleteImageAsync(string publicId);
    Task AddImageAsync(ProductImage? image);
    Task<ProductImage?> GetImageByIdAsync(int id);
   // Task<List<ProductImage?>> GetImageByIdProductIdAsync(int id);
   Task<IEnumerable<ImageDto>> GetImageByProductIdAsync(int productId);
}