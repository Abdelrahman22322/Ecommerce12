using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Domain.Entities;
using Ecommerce.Core.Domain.RepositoryContracts;

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
                    opt => opt.MapFrom(src =>  src.Discount.DiscountAmount))
                    
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

            //CreateMap<AddProductDto, Product>()
            //    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new Category { Name = src.Category }))
            //    .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => new Brand { Name = src.Brand }))
            //    .ForMember(dest => dest.Discounts,
            //        opt => opt.MapFrom(src => new Discount
            //        {  
            //            DiscountAmount = src.DiscountAmount,
            //            StartDate = src.DiscountStartDate,
            //            EndDate = src.DiscountEndDate
            //        }))
            //    .ForMember(dest => dest.ProductTags,
            //        opt => opt.MapFrom(src => src.Tags.Select(t => new ProductTag
            //        {
            //            Tag = new Tag { Name = t }
            //        }).ToList()))
            //    .ForMember(dest => dest.ProductAttributeValues,
            //        opt => opt.MapFrom(src => src.ProductAttributes.Select((pa, index) => new ProductAttributeValue
            //        {

            //            ProductAttribute = new ProductAttribute { Name = pa },
            //            Value = src.ProductAttributesValues[index]
            //
            // }).ToList()))
            //    //.ForMember(dest => dest.ProductImages,
            //    //    opt => opt.MapFrom(src => src.ProductImageUrls.Select(pi => new ProductImage
            //    //    {
            //    //        ImageUrl = pi
            //    //    }).ToList()))
            //    .ForPath(dest => dest.Supplier.CompanyName,
            //        opt => opt.MapFrom(src => src.CompanyName))
            //    .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
            //    .ForPath(dest => dest.Supplier.ContactName,
            //        opt => opt.MapFrom(src => src.ContactName));

            CreateMap<AddProductDto, Product>()
                .ForMember(dest => dest.Discount,
                    opt => opt.MapFrom(src => new Discount
                    {
                        DiscountName = src.DiscountName,
                        DiscountAmount = src.DiscountAmount,
                        StartDate = src.DiscountStartDate,
                        EndDate = src.DiscountEndDate
                    }))
                // لن نستخدم مابينج للـ Category و Brand
                .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
                .ForMember(dest => dest.ProductAttributeValues, opt => opt.Ignore())
                .ForPath(dest => dest.Supplier.CompanyName,
                    opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForPath(dest => dest.Supplier.ContactName,
                    opt => opt.MapFrom(src => src.ContactName))
                .ForMember(dest => dest.Category, opt => opt.Ignore()) // Ignore Category
                .ForMember(dest => dest.Brand, opt => opt.Ignore());


            CreateMap<CloudinaryResult, ProductImage>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.PublicId, opt => opt.MapFrom(src => src.PublicId));

            // 

            CreateMap<Category, CategoryDto>().ReverseMap();



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


            //CreateMap<Product, UpdateProductDto>()
            //    .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            //    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            //    .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            //    //    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.))
            //    .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            //    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            //    .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.UnitsInStock))
            //    .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.ReorderLevel))
            //    .ForMember(dest => dest.Discontinued, opt => opt.MapFrom(src => src.Discontinued))
            //    .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.ProductCode))
            //    .ForPath(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Supplier.CompanyName))
            //    .ForPath(dest => dest.ContactName, opt => opt.MapFrom(src => src.Supplier.ContactName))
            //    .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => src.IsArchived))
            //    .ForMember(dest => dest.Tags,
            //        opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag.Name).ToList()))
            //    .ForMember(dest => dest.ProductAttributes,
            //        opt => opt.MapFrom(src =>
            //            src.ProductAttributeValues.Select(pav => pav.ProductAttribute.Name).ToList()))
            //    .ForMember(dest => dest.ProductAttributesValues,
            //        opt => opt.MapFrom(src => src.ProductAttributeValues.Select(pav => pav.Value).ToList()))
            //    .ForMember(dest => dest.ImageUrls,
            //        opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl).ToList()))
            //    .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

            CreateMap<Product, UpdateProductDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                //  .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.UnitsInStock, opt => opt.MapFrom(src => src.UnitsInStock))
                .ForMember(dest => dest.ReorderLevel, opt => opt.MapFrom(src => src.ReorderLevel))
                .ForMember(dest => dest.Discontinued, opt => opt.MapFrom(src => src.Discontinued))
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.ProductCode))
                .ForPath(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.CompanyName : null))
                .ForPath(dest => dest.ContactName,
                    opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.ContactName : null))
                .ForMember(dest => dest.IsArchived, opt => opt.MapFrom(src => src.IsArchived))
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src =>
                        src.ProductTags != null
                            ? src.ProductTags.Select(pt => pt.Tag.Name).ToList()
                            : new List<string>()))
                .ForMember(dest => dest.ProductAttributes,
                    opt => opt.MapFrom(src =>
                        src.ProductAttributeValues != null
                            ? src.ProductAttributeValues.Select(pav => pav.ProductAttribute.Name).ToList()
                            : new List<string>()))
                .ForMember(dest => dest.ProductAttributesValues,
                    opt => opt.MapFrom(src =>
                        src.ProductAttributeValues != null
                            ? src.ProductAttributeValues.Select(pav => pav.Value).ToList()
                            : new List<string>()))
                .ForMember(dest => dest.ImageUrls,
                    opt => opt.MapFrom(src =>
                        src.ProductImages != null
                            ? src.ProductImages.Select(pi => pi.ImageUrl).ToList()
                            : new List<string>()))
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

            //CreateMap<Product, CartItemDto>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            //    .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            //    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            //    .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.ProductImages.FirstOrDefault().ImageUrl))
            //    .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
            //    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            //    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice));

            CreateMap<CartItem, ItemDto>()
                //   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductImage,
                    opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault().ImageUrl))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Product.Brand.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Product.Category.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.UnitPrice));


            CreateMap<WishlistItem, ItemDto>()
                //   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductImage,
                    opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault().ImageUrl))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Product.Brand.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Product.Category.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.UnitPrice));



            CreateMap<UserProfile, UpdateUserProfileDto>().ReverseMap();
            CreateMap<UserProfileDto, UserProfile>();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.Status))
                .ReverseMap();
            CreateMap<OrderStatus, OrderStatusDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();



            CreateMap<Shipper, ShipperDto>().ReverseMap();
            CreateMap<Shipping, ShippingDto>().ReverseMap();
            CreateMap<CheckoutDto, UpdateUserProfileDto>();
            CreateMap<CheckoutDto, UserProfile>();

            CreateMap<ShippingDto, Shipping>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore the Id property

            CreateMap<ItemDto, OrderDetail>();
            //  .ForMember(dest => dest.Product, opt => opt.Ignore()); // Ignore navigation property if necessar

            CreateMap<CartItem, OrderDetail>()
                //  .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore()) // You will set this manually
                .ForMember(dest => dest.DiscountAmount, opt => opt.Ignore()) // You will set this manually
                .ForMember(dest => dest.Price, opt => opt.Ignore()); // You will set this manually
            //  .ForMember(dest => dest.Product, opt => opt.Ignore()); // You will set this manually

            //CreateMap<OrderCreateDto, Order>()
            //    .ForMember(dest => dest., opt => opt.Ignore());
            CreateMap<OrderCreateDto, Order>();

            CreateMap<CheckoutDto, ShippingDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StreetAddress + ", " + src.City))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<UserProfile, ShippingDto>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StreetAddress + ", " + src.City))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));




            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<CreateCommentDto, Comment>();

            CreateMap<UpdateCommentDto, Comment>();

            CreateMap<Rating, RatingDto>().ReverseMap();


        }
    }
}

