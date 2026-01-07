using AutoMapper;
using StockMaster.Application.DTOs;
using StockMaster.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockMaster.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
