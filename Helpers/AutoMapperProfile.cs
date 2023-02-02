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
            CreateMap<User, ResponseManager>();
            CreateMap<RegisterUser, ResponseManager>();
            CreateMap<UpdateUser, ResponseManager>();
        }
        
    }
}
