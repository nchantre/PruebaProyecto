using AutoMapper;
using MediatR;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Queries.Owner;
using RealEstate.Application.Services;

namespace RealEstate.Application.Handlers.Owner
{
    public class GetOwnertByIdHandler : IRequestHandler<GetOwnertByIdQuery, ResponseOwnerDto>
    {
        private readonly OwnerService _service;
        private readonly IMapper _mapper;

        public GetOwnertByIdHandler(OwnerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<ResponseOwnerDto> Handle(GetOwnertByIdQuery query, CancellationToken ct)
        {
            //var ownert = await _service.GetByIdAsync(query.IdOwner)
            //    ?? throw new KeyNotFoundException("Owner not found");


            var ownert = await _service.GetByIdAsync(query.IdOwner);

            if (ownert == null)
            {
                throw new KeyNotFoundException($"Owner with Id {query.IdOwner} not found in DB");
            }
            return _mapper.Map<ResponseOwnerDto>(ownert);
        }
    }
}
