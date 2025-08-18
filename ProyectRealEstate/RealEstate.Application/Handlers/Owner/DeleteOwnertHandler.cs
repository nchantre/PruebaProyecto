using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Services;
using RealEstate.Domain.Interfaces;


namespace RealEstate.Application.Handlers.Owner
{
    public class DeleteOwnertHandler : IRequestHandler<DeleteOwnertCommand, bool>
    {

        private readonly IOwnerService _service;

        public DeleteOwnertHandler(IOwnerService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(DeleteOwnertCommand request, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(request.IdOwner);
            return true;
        }
    }
}
