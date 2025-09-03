using AutoMapper;
using JuniorBackDotnetProxy.Domain.Entities;
using JuniorBackDotnetProxy.Api.Dtos;

namespace JuniorBackDotnetProxy.Api.Profiles
{
   
    /// AutoMapper profile for Product and ProductDto.
    /// Defines how properties are mapped between the domain entity and DTO.
    
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
