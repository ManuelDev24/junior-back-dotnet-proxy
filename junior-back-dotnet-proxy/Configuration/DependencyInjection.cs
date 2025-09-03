using AutoMapper;
using JuniorBackDotnetProxy.Domain.Entities;
using JuniorBackDotnetProxy.Api.Dtos;

namespace JuniorBackDotnetProxy.Api.Mapping
{
    
    /// AutoMapper profile for mapping between Product entity and ProductDto.
    /// Enables automatic conversion of properties with the same names and types.
    
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Map from Product entity to ProductDto
            CreateMap<Product, ProductDto>();

            // Map from ProductDto back to Product entity
            CreateMap<ProductDto, Product>();
        }
    }
}
