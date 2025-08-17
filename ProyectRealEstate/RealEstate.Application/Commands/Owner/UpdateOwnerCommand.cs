using MediatR;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Owers.DTOs.Response;

namespace RealEstate.Application.Commands.Owner
{
    public class UpdateOwnertCommand : IRequest<ResponseOwnerDto>
    {
        public string IdOwner { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public DateTime Birthday { get; set; }
        public List<PropertyDto> Properties { get; set; } = new();
    }
}
