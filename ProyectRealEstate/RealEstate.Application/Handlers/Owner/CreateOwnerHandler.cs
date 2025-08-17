using AutoMapper;
using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Owers.DTOs.Request;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Handlers.Owner
{

    public class CreateOwnertHandler : IRequestHandler<CreateOwnertCommand, ResponseOwnerDto>
    {
        private readonly OwnerService _service;
        private readonly IMapper _mapper;

        public CreateOwnertHandler(OwnerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<ResponseOwnerDto> Handle(CreateOwnertCommand command, CancellationToken ct)
        {

            var ownert = _mapper.Map<Domain.Entities.Owner>(command);
            await _service.AddAsync(ownert);
            return _mapper.Map<ResponseOwnerDto>(ownert);
        }
    }

}




