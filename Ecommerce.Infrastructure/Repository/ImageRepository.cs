using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repository;

public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _context;
    private readonly Cloudinary _cloudinary;

    public ImageRepository(ApplicationDbContext context, Cloudinary cloudinary)
    {
        _context = context;
        _cloudinary = cloudinary;
    }

    public async Task<CloudinaryResult> UploadImageAsync(ImageUploadParams uploadParams)
    {
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return new CloudinaryResult
        {
            Url = uploadResult.SecureUrl.AbsoluteUri,
            PublicId = uploadResult.PublicId
        };
    }

    public async Task DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        if (result.Result == "ok")
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(img => img.PublicId == publicId);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task AddImageAsync(ProductImage image)
    {
        _context.ProductImages.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductImage> GetImageByIdAsync(int id)
    {
        return await _context.ProductImages.FindAsync(id);
    }
}