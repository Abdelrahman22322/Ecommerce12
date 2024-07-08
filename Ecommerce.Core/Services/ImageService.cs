using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<CloudinaryResult> UploadImageAsync(ImageUploadParams uploadParams)
    {
        return await _imageRepository.UploadImageAsync(uploadParams);
    }

    public async Task DeleteImageAsync(string publicId)
    {
        await _imageRepository.DeleteImageAsync(publicId);
    }

    public async Task AddImageAsync(ProductImage image)
    {
        await _imageRepository.AddImageAsync(image);
    }

    public async Task<ProductImage> GetImageByIdAsync(int id)
    {
        return await _imageRepository.GetImageByIdAsync(id);
    }
}