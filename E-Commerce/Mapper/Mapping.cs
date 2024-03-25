using AutoMapper;
using E_Commerce.DTO;
using E_Commerce.Models;

namespace E_Commerce.Mapper
{
    public class Mapping : Profile
    {

        public Mapping()
        {

            CreateMap<User, CreateUser>().ReverseMap();

            CreateMap<Product, ProductDTO>().ReverseMap();

        }
    }
}
