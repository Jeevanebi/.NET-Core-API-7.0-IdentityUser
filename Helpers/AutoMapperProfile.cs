using AutoMapper;
using WebService.API.Entity;
using WebService.API.Models;

namespace WebService.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterUser, User>();
            CreateMap<UpdateUser, User>();
        }
        
    }
}
