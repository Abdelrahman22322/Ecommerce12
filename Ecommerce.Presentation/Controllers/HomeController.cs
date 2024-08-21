using Ecommerce.Core.Domain.RepositoryContracts;
using Ecommerce.Core.ServicesContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetProductsByFilter([FromQuery] ProductFilter filter)
        {
            var products = await _productService.GetProductsByFilterAsync(filter);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string searchTerm)
        {
            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        [HttpGet("top-selling")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            var products = await _productService.GetTopSellingProductsAsync();
            return Ok(products);
        }

        [HttpGet("newest")]
        public async Task<IActionResult> GetNewestProducts()
        {
            var products = await _productService.GetNewestProductsAsync();
            return Ok(products);
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProducts()
        {
            var products = await _productService.GetFeaturedProductsAsync();
            return Ok(products);
        }

        [HttpGet("on-sale")]
        public async Task<IActionResult> GetProductsOnSale()
        {
            var products = await _productService.GetProductsOnSaleAsync();
            return Ok(products);
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetProductsBySupplier(int supplierId)
        {
            var products = await _productService.GetProductsBySupplierAsync(supplierId);
            return Ok(products);
        }

        [HttpGet("out-of-stock")]
        public async Task<IActionResult> GetOutOfStockProducts()
        {
            var products = await _productService.GetOutOfStockProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("cards")]
        public async Task<IActionResult> GetProductCards()
        {
            var productCards = await _productService.GetProductCards();
            return Ok(productCards);
        }


        [HttpGet("{productId}/similar")]
        public async Task<IActionResult> GetSimilarProducts(int productId)
        {
            var products = await _productService.GetSimilarProductsOrRelatedAsync(productId);
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        //[HttpGet("{productId}/related")]
        //public async Task<IActionResult> GetRelatedProducts(int productId)
        //{
        //    var products = await _productService.GetRelatedProductsAsync(productId);
        //    if (products == null || !products.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(products);
       // }
    }
}
