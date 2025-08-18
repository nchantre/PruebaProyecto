using AutoMapper;
using Moq;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Handlers.Owner;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Handlers
{
    [TestFixture]
    public class UpdateOwnertHandlerTests
    {
        private Mock<IOwnerService> _serviceMock;
        private IMapper _mapper;
        private UpdateOwnertHandler _handler;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IOwnerService>();

            // Configuración mínima de AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateOwnertCommand, Owner>();
                cfg.CreateMap<Owner, ResponseOwnerDto>();
            });
            _mapper = config.CreateMapper();

            _handler = new UpdateOwnertHandler(_serviceMock.Object, _mapper);
        }

        [Test]
        public async Task Handle_ShouldUpdateOwner_WhenOwnerExists()
        {
            // Arrange
            var existingOwner = new Owner
            {
                IdOwner = "1",
                Name = "Juan Pérez",
                Address = "Calle 123",
                Photo = "https://example.com/photo.jpg",
                Birthday = new DateTime(1990, 1, 1)
            };

            _serviceMock.Setup(s => s.GetByIdAsync("1"))
                        .ReturnsAsync(existingOwner);

            var command = new UpdateOwnertCommand
            {
                IdOwner = "1",
                Name = "Pedro Gómez",
                Address = "Avenida 456",
                Photo = "https://example.com/newphoto.jpg",
                Birthday = new DateTime(1995, 5, 5)
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Pedro Gómez"));
            Assert.That(result.Address, Is.EqualTo("Avenida 456"));

            _serviceMock.Verify(s => s.GetByIdAsync("1"), Times.Once);
            _serviceMock.Verify(s => s.UpdateAsync("1", It.IsAny<Owner>()), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenOwnerDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync("99"))
                        .ReturnsAsync((Owner)null);

            var command = new UpdateOwnertCommand
            {
                IdOwner = "99",
                Name = "No Existe",
                Address = "Sin dirección"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _handler.Handle(command, CancellationToken.None));

            Assert.That(ex.Message, Is.EqualTo("Owner not found"));
            _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<string>(), It.IsAny<Owner>()), Times.Never);
        }
    }
}
