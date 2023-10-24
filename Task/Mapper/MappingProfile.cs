using AutoMapper;
using DAL.Entities;
using ProductTask.Models;

namespace ProductTask.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
