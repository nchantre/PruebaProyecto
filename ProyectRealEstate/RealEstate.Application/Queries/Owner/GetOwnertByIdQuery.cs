using MediatR;
using RealEstate.Application.Owers.DTOs;

namespace RealEstate.Application.Queries.Owner
{
    public class GetOwnertByIdQuery : IRequest<OwnertDto>
    {
        public string IdOwner { get; set; } = default!;
    }
}
