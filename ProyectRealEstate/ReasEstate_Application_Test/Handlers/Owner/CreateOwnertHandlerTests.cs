using AutoMapper;
using Moq;
using NUnit.Framework;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Handlers.Owner;
using RealEstate.Application.Owers.DTOs.Response;
using RealEstate.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstate.Tests.Application.Handlers.Owner
{
    [TestFixture]
    public class CreateOwnertHandlerTests
    {
        private Mock<IOwnerService> _mockService;
        private Mock<IMapper> _mockMapper;
        private CreateOwnertHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IOwnerService>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateOwnertHandler(_mockService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldCallServiceAddAsync_AndReturnResponseDto()
        {
            // Arrange
            var command = new CreateOwnertCommand
            {
                Name = "Juan Pérez",
                Address = "Calle 123",
                Photo = "foto.jpg",
                Birthday = new System.DateTime(1990, 5, 20)
            };

            var ownerEntity = new Domain.Entities.Owner();
            var responseDto = new ResponseOwnerDto
            {
                Name = "Juan Pérez",
                Address = "Calle 123",
                Photo = "foto.jpg"
            };

            _mockMapper.Setup(m => m.Map<Domain.Entities.Owner>(command))
                       .Returns(ownerEntity);

            _mockMapper.Setup(m => m.Map<ResponseOwnerDto>(ownerEntity))
                       .Returns(responseDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockService.Verify(s => s.AddAsync(ownerEntity), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual("Juan Pérez", result.Name);
            Assert.AreEqual("Calle 123", result.Address);
            Assert.AreEqual("foto.jpg", result.Photo);
        }
    }
}
