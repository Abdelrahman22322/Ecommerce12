using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Core.ServicesContracts;

public interface IImageService
{
    Task<CloudinaryResult> UploadImageAsync(IFormFile uploadParams);
    Task DeleteImageAsync(string publicId);
    Task AddImageAsync(ProductImage? image);
    Task<ProductImage?> GetImageByIdAsync(int id);
     Task<List<ProductImage?>> GetImageByProductIdAsync(int id);
}