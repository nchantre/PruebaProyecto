using MediatR;
using RealEstate.Application.Commands.Owner;
using RealEstate.Domain.Repositories;

namespace RealEstate.Application.Handlers.Owner
{
    public class DeleteOwnertHandler : IRequestHandler<DeleteOwnertCommand, bool>
    {
        private readonly IOwnerRepositories _repository;

        public DeleteOwnertHandler(IOwnerRepositories repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteOwnertCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.IdOwner);
            return true; // o false si algo falla
        }
    }
}
