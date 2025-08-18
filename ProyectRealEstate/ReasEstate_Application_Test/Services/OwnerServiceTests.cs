using Moq;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class OwnerServiceTests
    {
        private Mock<IOwnerRepository> _repositoryMock;
        private OwnerService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IOwnerRepository>();
            _service = new OwnerService(_repositoryMock.Object);
        }

        [Test]
        public async Task GetAllAsync_Should_CallRepository_AndReturnOwners()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new Owner { IdOwner = "1", Name = "Juan Pérez", Address = "Calle 123" },
                new Owner { IdOwner = "2", Name = "Maria Gómez", Address = "Calle 456" }
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(owners);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_Should_CallRepository_AndReturnOwner()
        {
            // Arrange
            var owner = new Owner { IdOwner = "1", Name = "Juan Pérez" };

            _repositoryMock.Setup(r => r.GetByIdAsync("1"))
                .ReturnsAsync(owner);

            // Act
            var result = await _service.GetByIdAsync("1");

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Juan Pérez"));
            _repositoryMock.Verify(r => r.GetByIdAsync("1"), Times.Once);
        }

        [Test]
        public async Task AddAsync_Should_CallRepository()
        {
            // Arrange
            var owner = new Owner { IdOwner = "1", Name = "Juan Pérez" };

            // Act
            await _service.AddAsync(owner);

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(owner), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_Should_CallRepository()
        {
            // Arrange
            var owner = new Owner { IdOwner = "1", Name = "Juan Pérez" };

            // Act
            await _service.UpdateAsync("1", owner);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync("1", owner), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Should_CallRepository()
        {
            // Act
            await _service.DeleteAsync("1");

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync("1"), Times.Once);
        }
    }
}
