using AutoMapper;
using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Services;


namespace RealEstate.Application.Handlers.Owner
{
    public class UpdateOwnertHandler : IRequestHandler<UpdateOwnertCommand, OwnerDto>
    {
        private readonly OwnerService _service;

        private readonly IMapper _mapper;

        public UpdateOwnertHandler(OwnerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<OwnerDto> Handle(UpdateOwnertCommand command, CancellationToken ct)
        {
            var ownert = await _service.GetByIdAsync(command.IdOwner)
                ?? throw new KeyNotFoundException("Owner not found");

            _mapper.Map(command, ownert);
            await _service.UpdateAsync(ownert.IdOwner.ToString(), ownert);

            return _mapper.Map<OwnerDto>(ownert);
        }
    }
}
