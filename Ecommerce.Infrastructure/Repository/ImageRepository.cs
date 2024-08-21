using System.Security.Cryptography.X509Certificates;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Infrastructure.DbContext;
using Microsoft.AspNetCore.Http;
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

    public async Task<CloudinaryResult> UploadImageAsync(IFormFile uploadParam)
    {
        if (uploadParam == null || uploadParam.Length == 0)
        {
            throw new ArgumentException("Invalid form file.");
        }

        // Validate file type
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(uploadParam.FileName).ToLower();
        if (Array.IndexOf(allowedExtensions, fileExtension) < 0)
        {
            throw new ArgumentException("Unsupported file type.");
        }

        // Validate file size (example: max 5MB)
        const long maxFileSize = 5 * 1024 * 1024;
        if (uploadParam.Length > maxFileSize)
        {
            throw new ArgumentException("File size exceeds the maximum limit.");
        }

        try
        {

            using (var stream = uploadParam.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File =  new FileDescription(uploadParam.FileName, stream)


                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"Image upload failed: {uploadResult.Error.Message}");
                }

                return new CloudinaryResult
                {
                    Url = uploadResult.SecureUrl.AbsoluteUri,
                    PublicId = uploadResult.PublicId
                };
            }
        }
        catch (Exception ex)
        {
            // Log the exception (use a logging framework or console for simplicity)
            Console.WriteLine($"Exception during image upload: {ex.Message}");
            throw;
        }
    }




    public async Task DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
        {
            throw new ArgumentNullException(nameof(publicId));
        }

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

    public async Task AddImageAsync(ProductImage? image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        _context.ProductImages.Add(image);
        await _context.SaveChangesAsync();
    }

    public async Task<ProductImage?> GetImageByIdAsync(int id)
    {
        return await _context.ProductImages.FindAsync(id); 

    }

    //public async Task<IEnumerable<ImageDto>> GetImageByProductIdAsync(int productId)
    //{
    //    var searchParams = new ListResourcesByTagParams
    //    {
    //        Tag = $"product_{productId}",
    //        MaxResults = 30 // عدل العدد حسب الحاجة
    //    };

    //    var searchResult = await _cloudinary.ListResourcesByTagAsync(searchParams);

    //    if (searchResult.StatusCode != System.Net.HttpStatusCode.OK)
    //    {
    //        throw new Exception("Failed to fetch images from Cloudinary.");
    //    }

    //    var images = searchResult.Resources.Select(resource => new ImageDto
    //    {
    //        PublicId = resource.PublicId,
    //        Url = resource.SecureUrl.ToString()
    //    }).ToList();

    //    return images;
    // }

    public async Task<IEnumerable<ImageDto>> GetImageByProductIdAsync(int productId)
    {
        // Define the tag based on the product ID
        string tag = $"product_{productId}";

        // Use the ListResourcesByTagAsync method with the tag as a string
        var searchResult = await _cloudinary.ListResourcesByTagAsync(tag);

        if (searchResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Failed to fetch images from Cloudinary.");
        }

        var images = searchResult.Resources.Select(resource => new ImageDto
        {
            PublicId = resource.PublicId,
            Url = resource.SecureUrl.ToString()
        }).ToList();

        return images;
    }



}

