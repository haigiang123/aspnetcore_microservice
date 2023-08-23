using AutoMapper;
using Infrastructure.Mapping;
using Product.API.Entities;
using Shared.DTOs.Product;

namespace Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<ProductDto, CatalogProduct>();
            CreateMap<CreateProductDto, CatalogProduct>();
            CreateMap<CatalogProduct, CreateProductDto>();
            CreateMap<UpdateProductDto, ProductDto>();
            CreateMap<ProductDto, UpdateProductDto>();
            CreateMap<UpdateProductDto, CatalogProduct>()
                .IgnoreAllNonExisting();
        }
    }
}
