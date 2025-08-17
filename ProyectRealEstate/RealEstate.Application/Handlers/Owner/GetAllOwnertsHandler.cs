using AutoMapper;
using MediatR;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Queries.Owner;
using RealEstate.Application.Services;

namespace RealEstate.Application.Handlers.Owner
{
    public class GetAllOwnertsHandler : IRequestHandler<GetAllOwnertsQuery, List<ResponseOwnerDto>>
    {
        private readonly OwnerService _service;

        private readonly IMapper _mapper;

        public GetAllOwnertsHandler(OwnerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<List<ResponseOwnerDto>> Handle(GetAllOwnertsQuery query, CancellationToken ct)
        {
            var ownerts = await _service.GetAllAsync();
            return _mapper.Map<List<ResponseOwnerDto>>(ownerts);
        }
    }
}
