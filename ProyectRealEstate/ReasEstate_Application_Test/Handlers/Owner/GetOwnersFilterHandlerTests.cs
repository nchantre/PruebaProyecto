using AutoMapper;
using Moq;
using RealEstate.Application.Owers.DTOs.Filter;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Application.Queries.Owner;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Filter;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Handlers
{
    [TestFixture]
    public class GetOwnersFilterHandlerTests
    {
        private Mock<IOwnerService> _serviceMock;
        private Mock<IMapper> _mapperMock;
        private GetOwnersFilterHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IOwnerService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetOwnersFilterHandler(_serviceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_WhenCalled_MapsRequestAndReturnsMappedOwners()
        {
            // Arrange
            var searchParamsDto = new PropertySearchParamsDto { Name = "Juan" };
            var query = new GetOwnersFilterQuery { SearchParams = searchParamsDto };

            var searchParams = new PropertySearchParams { Name = "Juan" };
            var owners = new List<Owner>
            {
                new Owner { Name = "Juan", Address = "Calle 123" },
                new Owner { Name = "Pedro", Address = "Av. Siempre Viva" }
            };
            var mappedOwners = new List<ResponseOwnerDto>
            {
                new ResponseOwnerDto { Name = "Juan", Address = "Calle 123" },
                new ResponseOwnerDto { Name = "Pedro", Address = "Av. Siempre Viva" }
            };

            // Configuración de mocks
            _mapperMock.Setup(m => m.Map<PropertySearchParams>(searchParamsDto))
                       .Returns(searchParams);

            _serviceMock.Setup(s => s.GetBySpecificationAsync(searchParams))
                        .ReturnsAsync(owners);

            _mapperMock.Setup(m => m.Map<List<ResponseOwnerDto>>(owners))
                       .Returns(mappedOwners);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Juan", result[0].Name);
            Assert.AreEqual("Pedro", result[1].Name);

            _mapperMock.Verify(m => m.Map<PropertySearchParams>(searchParamsDto), Times.Once);
            _serviceMock.Verify(s => s.GetBySpecificationAsync(searchParams), Times.Once);
            _mapperMock.Verify(m => m.Map<List<ResponseOwnerDto>>(owners), Times.Once);
        }
    }
}
