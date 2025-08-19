using AutoMapper;
using MediatR;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Queries.Owner;
using RealEstate.Domain.Entities.Filter;
using RealEstate.Domain.Interfaces;

public class GetOwnersFilterHandler
    : IRequestHandler<GetOwnersFilterQuery, List<ResponseOwnerDto>>
{
    private readonly IOwnerService _service;
    private readonly IMapper _mapper;

    public GetOwnersFilterHandler(IOwnerService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<List<ResponseOwnerDto>> Handle(GetOwnersFilterQuery request, CancellationToken cancellationToken)
    {
        var ownert = _mapper.Map<PropertySearchParams>(request.SearchParams);
        var owners = await _service.GetBySpecificationAsync(ownert);

        Console.WriteLine(owners.GetType().FullName);
        return _mapper.Map<List<ResponseOwnerDto>>(owners);
       // return _mapper.Map<List<RequestOwnerDto>>(owners);

     
    }
}
