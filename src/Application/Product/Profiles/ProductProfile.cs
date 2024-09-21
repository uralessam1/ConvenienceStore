using ConvenienceStoreApi.Application.Product.Models;
using AutoMapper;
using ConvenienceStoreApi.Domain.Entities;
using ConvenienceStoreApi.Application.Product.Commands;
namespace ConvenienceStoreApi.Application.Product.Profiles;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Tbl_Product, ProductResponseModel>()
            .ReverseMap();
        CreateMap<CreateProductCommand, Tbl_Product>();
    }
}