using AutoMapper;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Models.UserModels;

namespace WebService.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseManager>();
            CreateMap<RegisterUser, UserResponseManager>();
            CreateMap<UpdateUser, UserResponseManager>();
        }
        
    }
}
