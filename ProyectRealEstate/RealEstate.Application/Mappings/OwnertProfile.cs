using AutoMapper;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Mappings
{
    public class OwnertProfile : Profile
    {
        public OwnertProfile()
        {
            // Entity -> DTO
            CreateMap<Ownert, OwnertDto>().ReverseMap();
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
            CreateMap<PropertyTrace, PropertyTraceDto>().ReverseMap();

            // Command -> Entity
            CreateMap<CreateOwnertCommand, Ownert>();
            CreateMap<UpdateOwnertCommand, Ownert>();
            CreateMap<PropertyDto, Property>();
            CreateMap<PropertyImageDto, PropertyImage>();
            CreateMap<PropertyTraceDto, PropertyTrace>();
        }
    }
}
