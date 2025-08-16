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
            CreateMap<Owner, OwnerDto>().ReverseMap();
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
            CreateMap<PropertyTrace, PropertyTraceDto>().ReverseMap();

            // Command -> Entity
            CreateMap<CreateOwnertCommand, Owner>();
            CreateMap<UpdateOwnertCommand, Owner>();
            CreateMap<PropertyDto, Property>();
            CreateMap<PropertyImageDto, PropertyImage>();
            CreateMap<PropertyTraceDto, PropertyTrace>();
        }
    }
}
