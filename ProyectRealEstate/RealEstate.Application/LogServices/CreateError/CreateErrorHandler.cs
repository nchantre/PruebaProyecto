using MediatR;
using Nuget.LogService.Models;
using Nuget.LogService.Services;

namespace RealEstate.Application.LogServices.CreateError
{
    public class CreateErrorHandler : IRequestHandler<CreateErrorCommand, bool>
    {

        private readonly ILogServices _service;

        public CreateErrorHandler(ILogServices service)
        {
            _service = service;
        }

        public async Task<bool> Handle(CreateErrorCommand command, CancellationToken cancellationToken)
        {
            bool createErrorResult = false;
            createErrorResult = await _service.CreateErrorAsync(new CreateErrorIn
            {
                SeverityID = command.SeverityID,
                Description = command.Description,
                UserID = command.UserID,
                TransactionID = command.TransactionID,
                Code = command.Code,
                Component = command.Component,
                Machine = command.Machine,
                Date = command.Date
            });
            return createErrorResult;
        }

    }
}
