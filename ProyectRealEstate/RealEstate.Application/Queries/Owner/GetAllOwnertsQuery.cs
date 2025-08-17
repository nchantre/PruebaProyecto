using MediatR;
using RealEstate.Application.Owers.DTOs.Response;

namespace RealEstate.Application.Queries.Owner
{
    public class GetAllOwnertsQuery : IRequest<List<ResponseOwnerDto>> { }
}
