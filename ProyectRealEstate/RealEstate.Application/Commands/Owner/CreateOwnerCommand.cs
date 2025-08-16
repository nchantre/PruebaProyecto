using MediatR;
using RealEstate.Application.Owers.DTOs;

namespace RealEstate.Application.Commands.Owner
{
    public class CreateOwnertCommand : IRequest<OwnerDto>
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public DateTime Birthday { get; set; }
        public List<PropertyDto> Properties { get; set; } = new();
    }
}
