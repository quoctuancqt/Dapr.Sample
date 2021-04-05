using AutoMapper;
using ProductService.Application.Entities;
using SharedKernel.Mapping;
using System;

namespace ProductService.Application.Dto
{
    public class ProductDto : IMapFrom<Product>
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>();
        }
    }
}
