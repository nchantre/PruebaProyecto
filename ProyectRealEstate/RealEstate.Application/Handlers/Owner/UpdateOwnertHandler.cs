using AutoMapper;
using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Domain.Repositories;

namespace RealEstate.Application.Handlers.Owner
{
    public class UpdateOwnertHandler : IRequestHandler<UpdateOwnertCommand, OwnertDto>
    {
        private readonly IOwnerRepositories _repository;
        private readonly IMapper _mapper;

        public UpdateOwnertHandler(IOwnerRepositories repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OwnertDto> Handle(UpdateOwnertCommand command, CancellationToken ct)
        {
            var ownert = await _repository.GetByIdAsync(command.IdOwner)
                ?? throw new KeyNotFoundException("Owner not found");

            _mapper.Map(command, ownert); // Actualiza los campos
            await _repository.UpdateAsync(ownert);

            return _mapper.Map<OwnertDto>(ownert);
        }
    }
}
