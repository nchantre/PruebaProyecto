using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RealEstate.API.Controllers;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Application.Queries.Owner;

namespace RealEstate.API.Tests.Controllers
{
    [TestFixture]
    public class RealEstateControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private RealEstateController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new RealEstateController(_mediatorMock.Object);
        }

        [Test]
        public async Task Create_Returns_Ok_With_OwnertDto()
        {
            var command = new CreateOwnertCommand { Name = "Test", Address = "Addr", Photo = "Photo", Birthday = DateTime.Today, Properties = new List<PropertyDto>() };
            var expectedDto = new OwnertDto { IdOwner = "1", Name = "Test", Address = "Addr", Photo = "Photo", Birthday = DateTime.Today, Properties = new List<PropertyDto>() };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(expectedDto);

            var result = await _controller.Create(command);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult!.Value, Is.EqualTo(expectedDto));
        }

        [Test]
        public async Task Update_Returns_Ok_With_OwnertDto()
        {
            var command = new UpdateOwnertCommand { Name = "Test", Address = "Addr", Photo = "Photo", Birthday = DateTime.Today, Properties = new List<PropertyDto>() };
            var expectedDto = new OwnertDto { IdOwner = "1", Name = "Test", Address = "Addr", Photo = "Photo", Birthday = DateTime.Today, Properties = new List<PropertyDto>() };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(expectedDto);

            var result = await _controller.Update("1", command);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult!.Value, Is.EqualTo(expectedDto));
            Assert.That(command.IdOwner, Is.EqualTo("1"));
        }

        [Test]
        public async Task Delete_Returns_NoContent()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteOwnertCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.Delete("1");

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task GetById_Returns_Ok_With_OwnertDto()
        {
            var expectedDto = new OwnertDto { IdOwner = "1", Name = "Test", Address = "Addr", Photo = "Photo", Birthday = DateTime.Today, Properties = new List<PropertyDto>() };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetOwnertByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedDto);

            var result = await _controller.GetById("1");

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult!.Value, Is.EqualTo(expectedDto));



        }

        [Test]
        public async Task GetAll_Returns_Ok_With_ListOfOwnertDto()
        {
            var expectedList = new List<OwnertDto>
            {
                new OwnertDto { IdOwner = "1", Name = "Test", Address = "Addr", Photo = "Photo", Birthday = DateTime.Today, Properties = new List<PropertyDto>() }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllOwnertsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedList);

            var result = await _controller.GetAll();

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult!.Value, Is.EqualTo(expectedList));
        }
    }
}
