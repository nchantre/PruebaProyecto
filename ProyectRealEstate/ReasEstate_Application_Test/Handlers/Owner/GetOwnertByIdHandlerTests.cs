using AutoMapper;
using Moq;
using RealEstate.Application.Handlers.Owner;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Queries.Owner;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Handlers
{
    [TestFixture]
    public class GetOwnertByIdHandlerTests
    {
        private Mock<IOwnerService> _serviceMock;
        private IMapper _mapper;
        private GetOwnertByIdHandler _handler;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IOwnerService>();

            // Configuración de AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Owner, ResponseOwnerDto>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetOwnertByIdHandler(_serviceMock.Object, _mapper);
        }

        [Test]
        public async Task Handle_ShouldReturnResponseOwnerDto_WhenOwnerExists()
        {
            // Arrange
            var owner = new Owner
            {
                IdOwner = "1",
                Name = "Juan Pérez",
                Address = "Calle 123",
                Photo = "https://example.com/photo.jpg",
                Birthday = new DateTime(1990, 1, 1)
            };

            _serviceMock.Setup(s => s.GetByIdAsync("1"))
                        .ReturnsAsync(owner);

            var query = new GetOwnertByIdQuery { IdOwner = "1" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Juan Pérez"));
            Assert.That(result.Address, Is.EqualTo("Calle 123"));

            _serviceMock.Verify(s => s.GetByIdAsync("1"), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowKeyNotFoundException_WhenOwnerDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync("99"))
                        .ReturnsAsync((Owner)null);

            var query = new GetOwnertByIdQuery { IdOwner = "99" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _handler.Handle(query, CancellationToken.None));

            Assert.That(ex.Message, Is.EqualTo("Owner with Id 99 not found in DB"));
            _serviceMock.Verify(s => s.GetByIdAsync("99"), Times.Once);
        }
    }
}
