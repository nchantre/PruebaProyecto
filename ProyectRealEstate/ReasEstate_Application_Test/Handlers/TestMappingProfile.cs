using AutoMapper;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Handlers
{
    public class TestMappingProfile : Profile
    {
        public TestMappingProfile()
        {
            CreateMap<CreateOwnertCommand, Owner>();
            CreateMap<Owner, ResponseOwnerDto>();
        }
    }
}

