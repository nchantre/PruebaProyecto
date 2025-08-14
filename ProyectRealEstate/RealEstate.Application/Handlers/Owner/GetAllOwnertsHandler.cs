using AutoMapper;
using MediatR;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Queries.Owner;
using RealEstate.Domain.Repositories;

namespace RealEstate.Application.Handlers.Owner
{
    public class GetAllOwnertsHandler : IRequestHandler<GetAllOwnertsQuery, List<OwnertDto>>
    {
        private readonly IOwnerRepositories _repository;
        private readonly IMapper _mapper;

        public GetAllOwnertsHandler(IOwnerRepositories repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OwnertDto>> Handle(GetAllOwnertsQuery query, CancellationToken ct)
        {
            var ownerts = await _repository.GetAllAsync();
            return _mapper.Map<List<OwnertDto>>(ownerts);
        }
    }
}
