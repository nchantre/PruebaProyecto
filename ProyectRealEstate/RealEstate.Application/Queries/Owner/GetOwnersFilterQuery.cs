using MediatR;
using RealEstate.Application.Owers.DTOs.Filter;
using RealEstate.Application.Owers.DTOs.Request;
using RealEstate.Application.Owers.DTOs.Response;

namespace RealEstate.Application.Queries.Owner
{
    public class GetOwnersFilterQuery : IRequest<List<ResponseOwnerDto>>
    {
        public PropertySearchParamsDto SearchParams { get; set; } = default!;
    }
}
