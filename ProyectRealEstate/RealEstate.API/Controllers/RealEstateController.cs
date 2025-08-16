using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Queries.Owner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateController : ControllerBase
    {

        private readonly IMediator _mediator;

        public RealEstateController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<OwnerDto>> Create(CreateOwnertCommand command) =>
            Ok(await _mediator.Send(command));

        [HttpPut("{id}")]
        public async Task<ActionResult<OwnerDto>> Update(string id, UpdateOwnertCommand command)
        {
            command.IdOwner = id;
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteOwnertCommand { IdOwner = id });
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetById(string id) =>
            Ok(await _mediator.Send(new GetOwnertByIdQuery { IdOwner = id }));

        [HttpGet]
        public async Task<ActionResult<List<OwnerDto>>> GetAll() =>
            Ok(await _mediator.Send(new GetAllOwnertsQuery()));



    }
}
