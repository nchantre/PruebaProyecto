using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs.Filter;
using RealEstate.Application.Owers.DTOs.Request;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Queries.Owner;

namespace RealEstate.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RealEstateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RealEstateController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Crear un nuevo propietario
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseOwnerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOwnertCommand command)
        {
            if (command == null)
                return BadRequest("El comando no puede ser nulo.");

            var result = await _mediator.Send(command);

            // Devuelve 201 Created con la URL del recurso creado
            return CreatedAtAction(nameof(GetById), new { id = result.Name, version = "1.0" }, result);
        }

        /// <summary>
        /// Actualizar un propietario existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseOwnerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateOwnertCommand command)
        {
            if (command == null)
                return BadRequest("El comando no puede ser nulo.");

            command.IdOwner = id;
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound($"No se encontró el propietario con Id = {id}");

            return Ok(result);
        }

        /// <summary>
        /// Eliminar un propietario
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _mediator.Send(new DeleteOwnertCommand { IdOwner = id });

            if (!deleted)
                return NotFound($"No se encontró el propietario con Id = {id}");

            return Ok(new ResponseDeleteDto { Id = id });
        }

        /// <summary>
        /// Obtener un propietario por Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseOwnerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetOwnertByIdQuery { IdOwner = id });

            if (result == null)
                return NotFound($"No se encontró el propietario con Id = {id}");

            return Ok(result);
        }

        /// <summary>
        /// Obtener todos los propietarios
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ResponseOwnerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllOwnertsQuery());
            return Ok(result);
        }


        /// <summary>
        /// Buscar propietarios según filtros de propiedades
        /// </summary>
        /// <param name="req">Parámetros de búsqueda de propiedades</param>
        /// <returns>Lista de propietarios filtrados</returns>
        [HttpPost("search")]
        [ProducesResponseType(typeof(IEnumerable<ResponseOwnerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchByProperties([FromBody] PropertySearchParamsDto req)
        {
            if (req == null)
                return BadRequest("Los parámetros de búsqueda no pueden ser nulos.");

            var query = new GetOwnersFilterQuery
            {
                SearchParams = req
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

    }
}
