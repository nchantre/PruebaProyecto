using AutoMapper;
using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Services;
using RealEstate.Domain.Interfaces;


namespace RealEstate.Application.Handlers.Owner
{
    public class UpdateOwnertHandler : IRequestHandler<UpdateOwnertCommand, ResponseOwnerDto>
    {
        private readonly IOwnerService _service;

        private readonly IMapper _mapper;

        public UpdateOwnertHandler(IOwnerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<ResponseOwnerDto> Handle(UpdateOwnertCommand command, CancellationToken ct)
        {
            var ownert = await _service.GetByIdAsync(command.IdOwner)
                ?? throw new KeyNotFoundException("Owner not found");

            _mapper.Map(command, ownert);
            await _service.UpdateAsync(ownert.IdOwner.ToString(), ownert);

            return _mapper.Map<ResponseOwnerDto>(ownert);
        }
    }
}
