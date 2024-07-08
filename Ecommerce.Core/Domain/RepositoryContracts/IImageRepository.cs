using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.Domain.RepositoryContracts;

public interface IImageRepository
{
    Task<CloudinaryResult> UploadImageAsync(ImageUploadParams uploadParams);
    Task DeleteImageAsync(string publicId);
    Task AddImageAsync(ProductImage image);
    Task<ProductImage> GetImageByIdAsync(int id);
}