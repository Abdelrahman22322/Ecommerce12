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
    internal class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Product, ProducCardtDTO>()
                .ForMember(dest => dest.Discount,
                    opt => opt.MapFrom(src => src.Discounts.FirstOrDefault()
                        .DiscountAmount))
                .ForMember(dest => dest.Rate,
                    opt => opt.MapFrom(src => src.Ratings.Count > 0
                        ? src.Ratings.Average(r => r.Value)
                        : 0))
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.ProductImages.FirstOrDefault()
                        .ImageUrl));


            CreateMap<Product, ProductDetailDTO>()
                .ForMember(dest => dest.Brand,
                    opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag.Name)
                        .ToList()))
                .ForMember(dest => dest.ProductImageUrls,
                    opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl)
                        .ToList()))
                .ForMember(dest => dest.Ratings,
                    opt => opt.MapFrom(src => src.Ratings.Select(r => r.Value.ToString())
                        .ToList()))
                .ForMember(dest => dest.Comments,
                    opt => opt.MapFrom(src => src.Comments.Select(c => c.Content)
                        .ToList()))
                .ForMember(dest => dest.Discounts,
                    opt => opt.MapFrom(src => src.Discounts.Select(d => d.DiscountAmount.ToString())
                        .ToList()));

        }

    }
}
