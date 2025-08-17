using AutoMapper;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Owers.DTOs.Request;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Domain.Entities;
using System.Net;

namespace RealEstate.Application.Mappings
{
    public class OwnertProfile : Profile
    {
        public OwnertProfile()
        {

            CreateMap<RequestOwnerDto, Owner>()
             .ForMember(dest => dest.IdOwner, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)) 
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday));
            CreateMap<RequestPropertyDto, Property>();



            CreateMap<Owner, ResponseOwnerDto>();
            CreateMap<Property, ResponsePropertyDto>();


            CreateMap<Owner, ResponsePropertyDto>();

            // Entity ->
            CreateMap<Owner, UpdateOwnertCommand>().ReverseMap();

            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
            CreateMap<PropertyTrace, PropertyTraceDto>().ReverseMap();

            // Command -> Entity
            CreateMap<CreateOwnertCommand, Owner>();
           // CreateMap<UpdateOwnertCommand, Owner>();

            CreateMap<PropertyImageDto, PropertyImage>();
            CreateMap<PropertyTraceDto, PropertyTrace>();



     



        }
}
}
