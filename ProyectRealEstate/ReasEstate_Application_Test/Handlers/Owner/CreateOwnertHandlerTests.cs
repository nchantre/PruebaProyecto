using AutoMapper;
using Moq;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Handlers.Owner;
using RealEstate.Application.Owers.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Repositories;

namespace RealEstate.Application.Tests.Handlers.Owner
{
    [TestFixture]
    public class CreateOwnertHandlerTests
    {
        private Mock<IOwnerRepositories> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private CreateOwnertHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IOwnerRepositories>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateOwnertHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_Should_Map_Command_To_Entity_And_Add_Async_And_Return_Dto()
        {
            // Arrange
            var command = new CreateOwnertCommand
            {
                Name = "John Doe",
                Address = "123 Main St",
                Photo = "photo.jpg",
                Birthday = new DateTime(1990, 1, 1),
                Properties = new List<PropertyDto>()
            };

            var ownertEntity = new Domain.Entities.Owner
            {
                IdOwner = "generated-id",
                Name = command.Name,
                Address = command.Address,
                Photo = command.Photo,
                Birthday = command.Birthday,
                Properties = new List<Property>()
            };

            var ownertDto = new OwnertDto
            {
                IdOwner = ownertEntity.IdOwner,
                Name = ownertEntity.Name,
                Address = ownertEntity.Address,
                Photo = ownertEntity.Photo,
                Birthday = ownertEntity.Birthday,
                Properties = new List<PropertyDto>()
            };

            _mapperMock.Setup(m => m.Map<Domain.Entities.Owner>(command)).Returns(ownertEntity);
            _repositoryMock.Setup(r => r.AddAsync(ownertEntity)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<OwnertDto>(ownertEntity)).Returns(ownertDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<Domain.Entities.Owner>(command), Times.Once);
            _repositoryMock.Verify(r => r.AddAsync(ownertEntity), Times.Once);
            _mapperMock.Verify(m => m.Map<OwnertDto>(ownertEntity), Times.Once);

            Assert.That(result, Is.EqualTo(ownertDto));
        }
    }
}
