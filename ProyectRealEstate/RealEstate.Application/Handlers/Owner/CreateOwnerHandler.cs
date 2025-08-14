using AutoMapper;
using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Repositories;

namespace RealEstate.Application.Handlers.Owner
{
    public class CreateOwnertHandler : IRequestHandler<CreateOwnertCommand, OwnertDto>
    {
        private readonly IOwnerRepositories _repository;
        private readonly IMapper _mapper;

        public CreateOwnertHandler(IOwnerRepositories repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OwnertDto> Handle(CreateOwnertCommand command, CancellationToken ct)
        {
            var ownert = _mapper.Map<Ownert>(command);
            //ownert.IdOwner = Guid.NewGuid().ToString();
            await _repository.AddAsync(ownert);
            return _mapper.Map<OwnertDto>(ownert);
        }
    }
}
