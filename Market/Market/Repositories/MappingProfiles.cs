using AutoMapper;
using Market.Models;
using Market.Models.DTO;

namespace Market.Repositories
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, DTOProduct>(MemberList.Destination).ReverseMap();
            CreateMap<Category, DTOCategory>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, DTOStorage>(MemberList.Destination).ReverseMap();
        }
    }
}
