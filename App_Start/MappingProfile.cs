using AutoMapper;
using Trippin_Website.DTOS;

namespace Trippin_Website.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Piese, PieseDTO>();
            Mapper.CreateMap<PieseDTO, PieseDTO>();

        }
    }
}