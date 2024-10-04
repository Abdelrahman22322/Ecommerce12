using System.Linq.Expressions;
using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.DTO;
using Ecommerce.Core.ServicesContracts;
using FluentValidation;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Ecommerce.Core.Services;

public class ProductServices : IProductService
{

    private readonly IGenericRepository<Product?> _productRepository;
    private readonly ITagService _tagService;
    private readonly IProductTagService _productTagService;
    private readonly  IProductRepository _productRepository1;

    private readonly IImageService _productImageService;
    private readonly IProductAttributeServices _productAttributeService;
    private readonly IProductAttributeValueServices _productAttributeValueService;
    private readonly IBrandServices _brandService;
    private readonly ICategoriesServices _categoryService;
    private readonly IImageService _imageService;
    private readonly IValidator<AddProductDto> _productValidator;
    private readonly IValidator<UpdateProductDto> _productValidator1;

    private readonly IMapper _Mapper;
    private readonly IDiscountService _discountService;


    public ProductServices( IGenericRepository<Product?> productRepository, IValidator<AddProductDto> productValidator, IImageService productImageService, IProductAttributeServices productAttributeService, IProductAttributeValueServices productAttributeValueService, IBrandServices brandDiscountService, ICategoriesServices categoryService, IImageService imageService, ITagService productTagService, IMapper productMapper, IProductRepository productRepository1, IProductTagService producTagService, IDiscountService discountService, IValidator<UpdateProductDto> productValidator1)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
        _productImageService = productImageService;
        _productAttributeService = productAttributeService;
        _productAttributeValueService = productAttributeValueService;
        _brandService = brandDiscountService;
        _categoryService = categoryService;
        _imageService = imageService;
        _tagService = productTagService;
        _Mapper = productMapper;
        _productRepository1 = productRepository1;
        _productTagService = producTagService;
        _discountService = discountService;
        _productValidator1 = productValidator1;
    }



    
    public async Task AddAsync(AddProductDto product)
    {

        
        
            using (var transaction = await _productRepository1.BeginTransactionAsync())
            {
                try
                {
                    // Validate the product DTO
                    var validationResult = _productValidator.Validate(product);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }

                    // Determine and set the category and brand
                    var category = await _categoryService.DetermineCategoryAsync(product.Category);
                    var brand = await _brandService.DetermineBrandAsync(product.Brand);

                    // Map AddProductDTO to Product entity
                    var newProduct = _Mapper.Map<Product>(product);
                    newProduct.CategoryId = category.Id;
                    newProduct.BrandId = brand.Id;
                    newProduct.CreatedDate = DateTime.Now;

                    // Add the product to the repository
                    await _productRepository.AddAsync(newProduct);
                await _productRepository.SaveAsync();

                // Handle product images
                if (product.ProductImages != null && product.ProductImages.Any())
                    {
                        foreach (var formFile in product.ProductImages)
                        {
                            var uploadResult = await _imageService.UploadImageAsync(formFile);
                            if (uploadResult == null)
                            {
                                throw new Exception("Image upload failed.");
                            }
                            var productImage = _Mapper.Map<ProductImage>(uploadResult);
                            productImage.ProductId = newProduct.ProductId;
                            await _productImageService.AddImageAsync(productImage);
                        }
                    }

                    // Handle product tags
                    if (product.Tags != null && product.Tags.Any())
                    {
                        foreach (var tag in product.Tags)
                        {
                            var tagEntity = await _tagService.DetermineTagAsync(tag);

                            // Check if ProductTag already exists
                            var existingProductTag = await _productTagService.DetermineProductTagAsync(newProduct.ProductId, tagEntity.Id);
                            if (existingProductTag == null)
                            {
                                var productTag = new ProductTag
                                {
                                    ProductId = newProduct.ProductId,
                                    TagId = tagEntity.Id
                                };
                                await _productTagService.AddAsync(productTag);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No product tags provided.");
                    }

                    // Handle product attributes and their values
                    if (product.ProductAttributes != null && product.ProductAttributesValues != null &&
                        product.ProductAttributes.Count == product.ProductAttributesValues.Count)
                    {
                        for (int i = 0; i < product.ProductAttributes.Count; i++)
                        {
                            var attribute = product.ProductAttributes[i];
                            var attributeValue = product.ProductAttributesValues[i];
                            var attributeEntity = await _productAttributeService.DetermineAttributeAsync(attribute);
                            var productAttributeValue = new ProductAttributeValue
                            {
                                ProductId = newProduct.ProductId,
                                ProductAttributeId = attributeEntity.Id,
                                Value = attributeValue
                            };
                            await _productAttributeValueService.AddProductAttributeValues(productAttributeValue);
                        }
                    }

                    await _productRepository1.CommitTransactionAsync();
                }
                catch (ValidationException ex)
                {
                    await _productRepository1.RollbackTransactionAsync();
                    throw new Exception("Validation error: " + ex.Message, ex);
                }
                catch (Exception ex)
                {
                    await _productRepository1.RollbackTransactionAsync();
                    throw new Exception("Error occurred while adding product: " + ex.Message, ex);
                }

            }
        

        //using (var transaction = await _productRepository1.BeginTransactionAsync())
        //{
        //    try
        //    {
        //        // Validate the product DTO
        //        var validationResult = _productValidator.Validate(product);
        //        if (!validationResult.IsValid)
        //        {
        //            throw new ValidationException(validationResult.Errors);
        //        }

        //        // Determine and set the category and brand
        //        var category = await _categoryService.DetermineCategoryAsync(product.Category);
        //        var brand = await _brandService.DetermineBrandAsync(product.Brand);

        //        // Map AddProductDTO to Product entity
        //        var newProduct = _Mapper.Map<Product>(product);
        //        newProduct.CategoryId = category.Id;
        //        newProduct.BrandId = brand.Id;
        //        newProduct.CreatedDate = DateTime.Now;

        //        // Add the product to the repository
        //        await _productRepository.AddAsync(newProduct);
        //        await _productRepository.SaveAsync();

        //        // Handle product images
        //        // Handle product images
        //        if (product.ProductImages != null)
        //        {
        //            foreach (var formFile in product.ProductImages)
        //            {
        //                var uploadResult = await _imageService.UploadImageAsync(formFile);
        //                if (uploadResult == null)
        //                {
        //                    throw new Exception("Image upload failed.");
        //                }

        //                var productImage = _Mapper.Map<ProductImage>(uploadResult);
        //                productImage.ProductId = newProduct.ProductId;
        //                await _productImageService.AddImageAsync(productImage);
        //                //var productImage = new ProductImage
        //                //{
        //                //    ImageUrl = uploadResult.Url.ToString(),
        //                //    PublicId = uploadResult.PublicId,
        //                //    ProductId = newProduct.ProductId
        //                //};
        //                //await _productImageService.AddImageAsync(productImage);

        //            }
        //        }



        //        // Handle product tags
        //        if (product.Tags != null)
        //        {
        //            foreach (var tag in product.Tags)
        //            {
        //                var tagEntity = await _tagService.DetermineTagAsync(tag);
        //                var productTag = new ProductTag
        //                {
        //                    ProductId = newProduct.ProductId,
        //                    TagId = tagEntity.Id
        //                };
        //                await _producTagService.AddAsync(productTag);
        //            }
        //        }

        //        // Handle product attributes and their values
        //        if (product.ProductAttributes != null && product.ProductAttributesValues != null)
        //        {
        //            for (int i = 0; i < product.ProductAttributes.Count; i++)
        //            {
        //                var attribute = product.ProductAttributes[i];
        //                var attributeValue = product.ProductAttributesValues[i];
        //                var attributeEntity = await _productAttributeService.DetermineAttributeAsync(attribute);
        //                var productAttributeValue = new ProductAttributeValue
        //                {
        //                    ProductId = newProduct.ProductId,
        //                    ProductAttributeId = attributeEntity.Id,
        //                    Value = attributeValue
        //                };
        //                await _productAttributeValueService.AddProductAttributeValues(productAttributeValue);
        //            }
        //        }

        //        await _productRepository1.CommitTransactionAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _productRepository1.RollbackTransactionAsync();
        //        // تسجيل الخطأ
        //        throw new Exception("Error occurred while adding product.", ex);
        //    }
        //}
    }

    public async Task UpdateAsync(int productId, UpdateProductDto productDto)
    {
        using (var transaction = await _productRepository1.BeginTransactionAsync())
        {
            try
            {
                // Validate the product DTO
                var validationResult = _productValidator1.Validate(productDto);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                // Retrieve the existing product from the database
                var existingProduct = await _productRepository.GetByIdAsync(productId);
                if (existingProduct == null)
                {
                    throw new Exception("Product not found.");
                }

                // Use AutoMapper to update the basic data of the product
                _Mapper.Map(productDto, existingProduct);
                existingProduct.CreatedDate = DateTime.Now;

                // Update product images
                if (productDto.ProductImages != null && productDto.ProductImages.Any())
                {
                    // Retrieve existing images for the product
                    var existingImages = await _imageService.GetImageByProductIdAsync(productId);

                    // Delete the old images using their publicId
                    foreach (var image in existingImages)
                    {
                        if (image?.PublicId != null)
                        {
                            await _imageService.DeleteImageAsync(image.PublicId);
                        }
                    }

                    // Add the new images
                    foreach (var formFile in productDto.ProductImages)
                    {
                        var uploadResult = await _imageService.UploadImageAsync(formFile);
                        if (uploadResult == null)
                        {
                            throw new Exception("Image upload failed.");
                        }
                        var productImage = _Mapper.Map<ProductImage>(uploadResult);
                        productImage.ProductId = productId;
                        await _productImageService.AddImageAsync(productImage);
                    }
                }

                // Update product attributes
                for (int i = 0; i < productDto.ProductAttributes.Count; i++)
                {
                    var attributeName = productDto.ProductAttributes[i];
                    var attributeValue = productDto.ProductAttributesValues[i];

                    var attribute = (await _productAttributeService.GetByNameAsync(attributeName));
                    if (attribute == null)
                    {
                        attribute = new ProductAttribute { Name = attributeName };
                        await _productAttributeService.AddProductAttribute(attribute);
                    }

                    var attributeValueEntity = await _productAttributeValueService.FindAsync(x => x.ProductId == productId && x.ProductAttributeId == attribute.Id);

                    if (attributeValueEntity != null && attributeValueEntity.Any())
                    {
                        foreach (var av in attributeValueEntity)
                        {
                            av.Value = attributeValue;
                            await _productAttributeValueService.UpdateAsync(av);
                        }
                    }
                    else
                    {
                        var newAttributeValue = new ProductAttributeValue
                        {
                            ProductId = productId,
                            ProductAttributeId = attribute.Id,
                            Value = attributeValue
                        };
                        await _productAttributeValueService.AddProductAttributeValues(newAttributeValue);
                    }
                }

                // Update product tags
                foreach (var tagName in productDto.Tags)
                {
                    var tag = await _tagService.FindAsync1(x => x.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };
                        await _tagService.AddAsync(tag);
                    }

                    var productTag = await _productTagService.FindAsync1(x => x.ProductId == productId && x.TagId == tag.Id);
                    if (productTag == null)
                    {
                        var newProductTag = new ProductTag
                        {
                            ProductId = productId,
                            TagId = tag.Id
                        };
                        await _productTagService.AddAsync(newProductTag);
                    }
                }

                //// Update discount if exists or do nothing
                //var existingDiscount = (await _discountService.FindAsync(d => d.ProductId == productId)).FirstOrDefault();
                //if (existingDiscount != null)
                //{
                //    existingDiscount.DiscountAmount = productDto.DiscountAmount;
                //    existingDiscount.StartDate = productDto.DiscountStartDate;
                //    existingDiscount.EndDate = productDto.DiscountEndDate;
                //    await _discountService.UpdateAsync(existingDiscount.DiscountId, existingDiscount);
                //}

                await _productRepository.UpdateAsync(existingProduct);
                await _productRepository.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (ValidationException ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Validation error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error occurred while updating product: " + ex.Message, ex);
            }
        }
    }




    public async Task DeleteAsync(int productId)
    {
        using (var transaction = await _productRepository1.BeginTransactionAsync())
        {
            try
            {
                // Retrieve the existing product from the database
                var existingProduct = await _productRepository.GetByIdAsync(productId);
                if (existingProduct == null)
                {
                    throw new Exception("Product not found.");
                }

                // Delete associated images
                var existingImages = await _imageService.GetImageByProductIdAsync(productId);
                foreach (var image in existingImages)
                {
                    if (image?.PublicId != null)
                    {
                        await _imageService.DeleteImageAsync(image.PublicId);
                    }
                }

                // Delete associated attribute values
                var attributeValueEntities = await _productAttributeValueService.FindAsync(x => x.ProductId == productId);
                foreach (var attributeValue in attributeValueEntities)
                {
                    await _productAttributeValueService.DeleteAsync(attributeValue.ProductId,attributeValue.Id);
                }

                // Delete associated tags
                var productTags = await _productTagService.FindAsync(x => x.ProductId == productId);
                foreach (var productTag in productTags)
                {
                    await _productTagService.DeleteAsync(productTag.ProductId, productTag.TagId);
                }

                // Delete associated discount
                var discounts = await _discountService.FindAsync(d => d.ProductId == productId);
                foreach (var discount in discounts)
                {
                    await _discountService.DeleteAsync(discount.DiscountId);
                }

                // Delete the product
                await _productRepository.DeleteAsync(existingProduct);
                await _productRepository.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error occurred while deleting product: " + ex.Message, ex);
            }
        }

    }



    public async Task<IEnumerable<ProducCardtDTO>> GetProductsByFilterAsync(ProductFilter filter)
    {
        var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetProductsByFilterAsync(filter));
        if (map == null)
        { 

            return _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository.GetAllAsync(predicate: null, includeword: null));


        }


       return map;


    }

    public async Task<IEnumerable<ProducCardtDTO>> SearchProductsAsync(string searchTerm)
    {  
        var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.SearchProductsAsync(searchTerm));
        if (map == null)
        {
            return _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository.GetAllAsync(predicate: null, includeword: null));
        }
        return map;
    }


    public async Task<IEnumerable<ProducCardtDTO>> GetTopSellingProductsAsync()
    {

      var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetTopSellingProductsAsync());
        return map;
    }
    public async Task<IEnumerable<ProducCardtDTO>> GetNewestProductsAsync()
    {
       
        var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetNewestProductsAsync());
        return map;
    }
    public async Task<IEnumerable<ProducCardtDTO>> GetFeaturedProductsAsync()
    {

        var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetFeaturedProductsAsync());
        return map;
    }
    public async Task<IEnumerable<ProducCardtDTO>> GetProductsOnSaleAsync()
    {

        var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetProductsOnSaleAsync());
        return map;
    }
    public async Task<IEnumerable<ProducCardtDTO>> GetProductsBySupplierAsync(int supplierId)
    {

        var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetProductsBySupplierAsync(supplierId));
        return map;
    }
    public async Task<IEnumerable<ProducCardtDTO>> GetOutOfStockProductsAsync()
    {

        var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetOutOfStockProductsAsync());
        return map;
    }

    public async Task<IEnumerable<ProductDetailDTO>> GetProducts()
    {

        return await _productRepository1.GetProducts();

    }

    public async Task<IEnumerable<ProducCardtDTO>> GetProductCards()
    {
        //var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository.GetAllAsync(predicate: null, includeword: "Category,Brand,Supplier,ProductImage"));
        //return map;
        return await _productRepository1.GetProductCards();
    }

    public async Task<ProductDetailDTO> ArchiveProduct(int productId)
    {


        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        product.IsArchived = true;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveAsync();

        return _Mapper.Map<ProductDetailDTO>(product);
    }

    public async Task<ProductDetailDTO> UnArchiveProduct(int productId)
    {

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        product.IsArchived = false;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveAsync();

        return _Mapper.Map<ProductDetailDTO>(product);
    }

  
        public async Task<UpdateProductDto> GetProductForUpdate(int productId)
        {

            var product = await _productRepository1.GetByIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            var productDto = _Mapper.Map<UpdateProductDto>(product);// Use AutoMapper to update the basic data of the product
          _Mapper.Map(productDto, product);

        //// Fetch related entities in parallel
        //    var categoryTask = _categoryService.GetByIdAsync(product.CategoryId);
        //  // var brandTask = _brandService.GetByIdAsync(product.BrandId);
        //var imagesTask = _imageService.GetImageByProductIdAsync(productId);
        //    var tagsTask = _productTagService.FindAsync(x => x.ProductId == productId);
        //    var attributesTask = _productAttributeValueService.FindAsync(x => x != null && x.ProductId == productId);

        //   await Task.WhenAll(imagesTask,categoryTask, tagsTask, attributesTask);

        //     var category = await categoryTask;
        //  //var brand = await brandTask;
        //  var images = await imagesTask;
        //  var tags = await tagsTask;
        //  var attributes = await attributesTask;

        // Map to DTO


        //productDto.Category = category?.Name;
        //productDto.Brand = brand?.Name;
        //productDto.ImageUrls = images?.Select(img => img.PublicId).ToList() ?? new List<string>();
        //productDto.Tags = tags?.Select(tag => tag.Tag.Name).ToList() ?? new List<string>();
        //productDto.ProductAttributes = attributes?.Select(attr => attr.ProductAttribute.Name).ToList() ?? new List<string>();
        //productDto.ProductAttributesValues = attributes?.Select(attr => attr.Value).ToList() ?? new List<string>();

        return productDto;
        }

        public async Task<IEnumerable<ProducCardtDTO>> GetRecommendedProductsAsync()
        {
            var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetRecommendedProductsAsync());

        return map;
    }

        public async Task<IEnumerable<ProducCardtDTO>> GetSimilarProductsOrRelatedAsync(int productId)
        {
            var map = _Mapper.Map<IEnumerable<ProducCardtDTO>>(await _productRepository1.GetSimilarOrRelatedProductsAsync(productId, true));
        return map;
    }

    


        public async Task<byte[]> ExportPdfAsync()
        {

        IEnumerable<Product> products = await _productRepository.GetAllAsync(predicate: null, includeword: "Category,Brand,Supplier");
        var map = _Mapper.Map<IEnumerable<ProductDetailDTO>>(products);
        var productslist = map.ToList();

        using var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Verdana", 12);

        for (int i = 0; i < productslist.Count ; i++){

            gfx.DrawString($"{productslist[i].ProductName} - {productslist[i].UnitPrice }  ", font, XBrushes.Black, new XRect(0, i * 20, page.Width, page.Height), XStringFormats.TopLeft);
        }

        using var stream = new MemoryStream();
        document.Save(stream);
        return stream.ToArray();




        }

}








