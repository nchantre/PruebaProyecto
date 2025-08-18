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
    public class GetAllOwnertsHandlerTests
    {
        
        private Mock<IOwnerService> _serviceMock;
        private IMapper _mapper;
        private GetAllOwnertsHandler _handler;

        [SetUp]
        public void Setup()
        {
            // Mock del servicio
            _serviceMock = new Mock<IOwnerService>();

            // Configuración de AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Owner, ResponseOwnerDto>();
            });
            _mapper = config.CreateMapper();

            _handler = new GetAllOwnertsHandler(_serviceMock.Object, _mapper);
        }

        [Test]
        public async Task Handle_ShouldReturnListOfResponseOwnerDto_WhenOwnersExist()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new Owner { IdOwner = "1", Name = "Juan Pérez", Address = "Calle 123", Photo = "https://example.com/photo1.jpg", Birthday = new DateTime(1990, 1, 1) },
                new Owner { IdOwner = "2", Name = "María Gómez", Address = "Avenida 456", Photo = "https://example.com/photo2.jpg", Birthday = new DateTime(1985, 5, 10) }
            };

            _serviceMock.Setup(s => s.GetAllAsync())
                        .ReturnsAsync(owners);

            var query = new GetAllOwnertsQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Juan Pérez"));
            Assert.That(result[1].Name, Is.EqualTo("María Gómez"));

            _serviceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }
    }
}

