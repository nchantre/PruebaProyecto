using Moq;
using NUnit.Framework;
using RealEstate.Application.Commands.Owner;
using RealEstate.Application.Handlers.Owner;
using RealEstate.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstate.Tests.Handlers
{
    [TestFixture]
    public class DeleteOwnertHandlerTests
    {
        private Mock<IOwnerService> _serviceMock;
        private DeleteOwnertHandler _handler;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IOwnerService>();
            _handler = new DeleteOwnertHandler(_serviceMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnTrue_WhenOwnerIsDeleted()
        {
            // Arrange
            var command = new DeleteOwnertCommand { IdOwner = "123" };

            // Simula que el servicio elimina correctamente
            _serviceMock.Setup(s => s.DeleteAsync(command.IdOwner))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result);
            _serviceMock.Verify(s => s.DeleteAsync("123"), Times.Once);
        }
    }
}
