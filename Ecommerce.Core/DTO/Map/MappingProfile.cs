using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Domain.Entities;

namespace Ecommerce.Core.DTO.Map
{
      public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Product, ProducCardtDTO>()
                .ForMember(dest => dest.Discount,
                    opt => opt.MapFrom(src => src.Discounts
                       ))
                .ForMember(dest => dest.Rate,
                    opt => opt.MapFrom(src => src.Ratings.Count > 0
                        ? src.Ratings.Average(r => r.Value)
                        : 0))
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.ProductImages.FirstOrDefault()
                        .ImageUrl));

            CreateMap<Product, ProductDetailDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.CompanyName))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.UnitsInStock))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Ratings.Count > 0
                                   ? src.Ratings.Average(r => r.Value)
                                                      : 0))
                .ForMember(dest => dest.Discontinued, opt => opt.MapFrom(src => src.Discontinued));

            CreateMap<AddProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Name = src.Category }))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => new Brand { Name = src.Brand }))
                .ForMember(dest => dest.Discounts,
                    opt => opt.MapFrom(src => new Discount
                    {
                        DiscountAmount = src.DiscountAmount,
                        StartDate = src.DiscountStartDate,
                        EndDate = src.DiscountEndDate
                    }))
                .ForMember(dest => dest.ProductTags,
                    opt => opt.MapFrom(src => src.Tags.Select(t => new ProductTag
                    {
                        Tag = new Tag { Name = t }
                    }).ToList()))
                .ForMember(dest => dest.ProductAttributeValues,
                    opt => opt.MapFrom(src => src.ProductAttributes.Select((pa, index) => new ProductAttributeValue
                    {
                        ProductAttribute = new ProductAttribute { Name = pa },
                        Value = src.ProductAttributesValues[index]
                    }).ToList()))
                //.ForMember(dest => dest.ProductImages,
                //    opt => opt.MapFrom(src => src.ProductImageUrls.Select(pi => new ProductImage
                //    {
                //        ImageUrl = pi
                //    }).ToList()))
                .ForPath(dest => dest.Supplier.CompanyName,
                    opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForPath(dest => dest.Supplier.ContactName,
                    opt => opt.MapFrom(src => src.ContactName));

            CreateMap<CloudinaryResult, ProductImage>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.PublicId, opt => opt.MapFrom(src => src.PublicId));







            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Name = src.Category }))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => new Brand { Name = src.Brand }))
               
                .ForMember(dest => dest.ProductTags,
                    opt => opt.MapFrom(src => src.Tags.Select(t => new ProductTag
                    {
                        Tag = new Tag { Name = t }
                    }).ToList()))
                .ForMember(dest => dest.ProductAttributeValues,
                    opt => opt.MapFrom(src => src.ProductAttributes.Select((pa, index) => new ProductAttributeValue
                    {
                        ProductAttribute = new ProductAttribute { Name = pa },
                        Value = src.ProductAttributesValues[index]
                    }).ToList()))
                //.ForMember(dest => dest.ProductImages,
                //    opt => opt.MapFrom(src => src.ProductImageUrls.Select(pi => new ProductImage
                //    {
                //        ImageUrl = pi
                //    }).ToList()))
                .ForPath(dest => dest.Supplier.CompanyName,
                    opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForPath(dest => dest.Supplier.ContactName,
                    opt => opt.MapFrom(src => src.ContactName));

            CreateMap<CloudinaryResult, ProductImage>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.PublicId, opt => opt.MapFrom(src => src.PublicId));
        }
    }
}
