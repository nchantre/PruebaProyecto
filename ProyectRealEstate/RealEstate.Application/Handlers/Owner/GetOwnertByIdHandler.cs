using AutoMapper;
using MediatR;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Queries.Owner;
using RealEstate.Domain.Repositories;

namespace RealEstate.Application.Handlers.Owner
{
    public class GetOwnertByIdHandler : IRequestHandler<GetOwnertByIdQuery, OwnertDto>
    {
        private readonly IOwnerRepositories _repository;
        private readonly IMapper _mapper;

        public GetOwnertByIdHandler(IOwnerRepositories repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OwnertDto> Handle(GetOwnertByIdQuery query, CancellationToken ct)
        {
            var ownert = await _repository.GetByIdAsync(query.IdOwner)
                ?? throw new KeyNotFoundException("Owner not found");
            return _mapper.Map<OwnertDto>(ownert);
        }
    }
}
