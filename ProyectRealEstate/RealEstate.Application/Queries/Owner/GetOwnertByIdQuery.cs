using MediatR;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Owers.DTOs.Response;

namespace RealEstate.Application.Queries.Owner
{
    public class GetOwnertByIdQuery : IRequest<ResponseOwnerDto>
    {
        public string IdOwner { get; set; } = default!;
    }
}

