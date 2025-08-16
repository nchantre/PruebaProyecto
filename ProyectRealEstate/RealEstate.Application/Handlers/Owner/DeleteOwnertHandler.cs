using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Services;


namespace RealEstate.Application.Handlers.Owner
{
    public class DeleteOwnertHandler : IRequestHandler<DeleteOwnertCommand, bool>
    {

        private readonly OwnerService _service;

        public DeleteOwnertHandler(OwnerService service)
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
