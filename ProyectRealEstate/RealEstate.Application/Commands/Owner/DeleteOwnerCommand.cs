using MediatR;

namespace RealEstate.Application.Commands.Owner
{
    public class DeleteOwnertCommand : IRequest<bool>
    {
        public string IdOwner { get; set; } = default!;
    }
}
