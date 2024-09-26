using AutoMapper;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.DTO;
using Ecommerce.Core.ServicesContracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers
{
    using FluentValidation;
    using global::Ecommerce.Core.Domain.RepositoryContracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    namespace Ecommerce.Presentation.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ProductController : ControllerBase
        {
            private readonly IProductService _productService;
            private readonly IImageRepository _imageRepository;

            public ProductController(IProductService productService, IImageRepository imageRepository)
            {
                _productService = productService;
                _imageRepository = imageRepository;
            }

            [HttpPost]

            public async Task<IActionResult> AddProduct([FromForm] AddProductDto productDto)
            {
                if (productDto == null)
                    return BadRequest("Product data is null.");

                // Handle image uploads
                var imageResults = new List<CloudinaryResult>();
                if (productDto.ProductImages != null && productDto.ProductImages.Count > 0)
                {
                    foreach (var file in productDto.ProductImages)
                    {
                        var imageResult = await _imageRepository.UploadImageAsync(file);
                        if (imageResult != null)
                        {
                            imageResults.Add(imageResult);
                        }
                    }
                }

                // Add image URLs to the product DTO
                productDto.ImageUrls = imageResults.Select(ir => ir.Url).ToList();

                try
                {
                    await _productService.AddAsync(productDto);
                    return Ok();
                }
                catch (ValidationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }

            [HttpPut("{productId}")]
            public async Task<IActionResult> UpdateProduct(int productId, [FromForm] UpdateProductDto productDto)
            {
                try
                {
                    await _productService.UpdateAsync(productId, productDto);
                    return Ok("Product updated successfully.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpDelete("{productId}")]
            public async Task<IActionResult> DeleteProduct(int productId)
            {
                try
                {
                    await _productService.DeleteAsync(productId);
                    return Ok("Product deleted successfully.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet]
            public async Task<IActionResult> GetProducts()
            {
                var products = await _productService.GetProducts();
                return Ok(products);
            }


            [HttpPost("archive/{productId}")]
            public async Task<IActionResult> ArchiveProduct(int productId)
            {
                var product = await _productService.ArchiveProduct(productId);
                return Ok(product);
            }

            [HttpPost("unarchive/{productId}")]
            public async Task<IActionResult> UnArchiveProduct(int productId)
            {
                var product = await _productService.UnArchiveProduct(productId);
                return Ok(product);
            }


            [HttpGet("export-pdf")]
            public async Task<IActionResult> ExportPdf()
            {
                var pdfBytes = await _productService.ExportPdfAsync();
                return File(pdfBytes, "application/pdf", "Products.pdf");
            }

            [HttpGet("{productId}")]
            public async Task<IActionResult> GetProductForUpdate(int productId)
            {
                try
                {
                    var productDto = await _productService.GetProductForUpdate(productId);
                    return Ok(productDto);
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }

    }
}



