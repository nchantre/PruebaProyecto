using MediatR;
using RealEstate.Application.Owers.DTOs;

namespace RealEstate.Application.Queries.Owner
{
    public class GetAllOwnertsQuery : IRequest<List<OwnertDto>> { }
}
