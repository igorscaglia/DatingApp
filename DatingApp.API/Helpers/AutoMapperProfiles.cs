using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Propriedades com o mesmo nome e tipo são automaticamente mapeadas
            CreateMap<User, UserForList>()
                .ForMember(dest => dest.PhotoUrl,
                        opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age,
                        opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForDetailed>()
                .ForMember(dest => dest.PhotoUrl,
                        opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age,
                        opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoForDetailed>();
        }
    }
}