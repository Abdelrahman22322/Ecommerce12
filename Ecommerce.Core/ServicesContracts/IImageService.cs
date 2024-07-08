using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.ServicesContracts;

public interface IImageService
{
    Task<CloudinaryResult> UploadImageAsync(ImageUploadParams uploadParams);
    Task DeleteImageAsync(string publicId);
    Task AddImageAsync(ProductImage image);
    Task<ProductImage> GetImageByIdAsync(int id);
}