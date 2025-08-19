using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RealEstate.API.Controllers;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs.Filter;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Queries.Owner;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstate.API.Tests
{
    [TestFixture]
    public class RealEstateControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private RealEstateController _controller;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new RealEstateController(_mediatorMock.Object);
        }

        [Test]
        public async Task Create_ReturnsCreatedAtAction_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateOwnertCommand { Name = "Juan Perez" };
            var response = new ResponseOwnerDto { IdOwner = "1", Name = "Juan Perez" };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Create(command) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("GetById", result.ActionName);
            Assert.AreEqual(response, result.Value);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenCommandIsNull()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_ReturnsOk_WhenOwnerExists()
        {
            // Arrange
            var command = new UpdateOwnertCommand { IdOwner = "1", Name = "Pedro" };
            var response = new ResponseOwnerDto { IdOwner = "1", Name = "Pedro" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateOwnertCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Update("1", command) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(response, result.Value);
        }

        [Test]
        public async Task Update_ReturnsNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateOwnertCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ResponseOwnerDto)null);

            // Act
            var result = await _controller.Update("99", new UpdateOwnertCommand()) as NotFoundObjectResult;

            // Assert
            Assert.IsNotNull(result);
            StringAssert.Contains("No se encontró el propietario", result.Value.ToString());
        }

        [Test]
        public async Task Delete_ReturnsOk_WhenDeleted()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteOwnertCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = (OkObjectResult)await _controller.Delete("123");
            var value = result.Value as ResponseDeleteDto;
            Assert.That(value!.Id, Is.EqualTo("123"));
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenNotDeleted()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteOwnertCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete("1");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetById_ReturnsOk_WhenOwnerExists()
        {
            // Arrange
            var response = new ResponseOwnerDto { IdOwner = "1", Name = "Juan" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetOwnertByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetById("1") as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(response, result.Value);
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenOwnerDoesNotExist()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetOwnertByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ResponseOwnerDto)null);

            // Act
            var result = await _controller.GetById("99");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetAll_ReturnsOk_WithList()
        {
            // Arrange
            var response = new List<ResponseOwnerDto>
            {
                new ResponseOwnerDto { IdOwner = "1", Name = "Juan" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllOwnertsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(response, result.Value);
        }


        [Test]
        public async Task SearchByProperties_WhenRequestIsNull_ReturnsBadRequest()
        {
            // Arrange
            PropertySearchParamsDto req = null;

            // Act
            var result = await _controller.SearchByProperties(req);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.AreEqual("Los parámetros de búsqueda no pueden ser nulos.", badRequest.Value);
        }

        [Test]
        public async Task SearchByProperties_WhenValidRequest_ReturnsOkWithData()
        {
            // Arrange
            var req = new PropertySearchParamsDto
            {
                Name = "Juan",
                Address = "Calle 123",
                Page = 1,
                PageSize = 10
            };

            var expectedOwners = new List<ResponseOwnerDto>
            {
                new ResponseOwnerDto { Name = "Juan", Address = "Calle 123" },
                new ResponseOwnerDto { Name = "Pedro", Address = "Av Siempre Viva" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetOwnersFilterQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOwners);

            // Act
            var result = await _controller.SearchByProperties(req);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<IEnumerable<ResponseOwnerDto>>(okResult.Value);

            var owners = okResult.Value as IEnumerable<ResponseOwnerDto>;
            CollectionAssert.AreEquivalent(expectedOwners, owners);
        }

    }
}
