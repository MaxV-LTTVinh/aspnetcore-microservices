using AutoMapper;
using Infrastructure.Mappings;
using Product.API.Entities;
using Shared.DTOs.Product;

namespace Product.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogProduct, ProductDto>().ReverseMap();
        CreateMap<CreateProductDto, CatalogProduct>().ReverseMap();
        CreateMap<UpdateProductDto, CatalogProduct>().ReverseMap()
        .IgnoreAllNonExisting();
    }
}