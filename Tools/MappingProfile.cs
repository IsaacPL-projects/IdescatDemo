using AutoMapper;
using PruebaBackend.Model.Context;
using PruebaBackend.Model.DTO;

namespace PruebaBackend.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Inhabitant, InhabitantDTO>();
            CreateMap<InhabitantDTO, Inhabitant>();
        }
    }
}