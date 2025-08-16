using MongoDB.Driver;
using Moq;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace RealEstate.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class OwnerRepositoriesTests
    {
        private Mock<IMongoCollection<Owner>> _collectionMock;
        private Mock<IMongoDatabase> _databaseMock;
        private OwnerRepositories _repository;

        [SetUp]
        public void SetUp()
        {
            _collectionMock = new Mock<IMongoCollection<Owner>>();
            _databaseMock = new Mock<IMongoDatabase>();
            _databaseMock.Setup(db => db.GetCollection<Owner>("Owner", null))
                .Returns(_collectionMock.Object);

            _repository = new OwnerRepositories(_databaseMock.Object);
        }

        [Test]
        public async Task AddAsync_Calls_InsertOneAsync()
        {
            var ownert = new Owner { IdOwner = "1", Name = "Test", Address = "Addr", Photo = "Photo" };
            await _repository.AddAsync(ownert);

            _collectionMock.Verify(c => c.InsertOneAsync(ownert, null, default), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_Calls_ReplaceOneAsync()
        {
            var ownert = new Owner { IdOwner = "1", Name = "Test", Address = "Addr", Photo = "Photo" };
            await _repository.UpdateAsync(ownert);

            _collectionMock.Verify(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Owner>>(),
                ownert,
                It.IsAny<ReplaceOptions>(),
                default), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Calls_DeleteOneAsync()
        {
            await _repository.DeleteAsync("1");

            _collectionMock.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Owner>>(),
                default), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_Calls_Find_And_FirstOrDefaultAsync()
        {
            var findFluentMock = new Mock<IAsyncCursor<Owner>>();
            var findMock = new Mock<IFindFluent<Owner, Owner>>();
            findMock.Setup(f => f.FirstOrDefaultAsync(default)).ReturnsAsync(new Owner { IdOwner = "1" });

            _collectionMock.Setup(c => c.Find(It.IsAny<FilterDefinition<Owner>>(), null))
                .Returns(findMock.Object);

            var result = await _repository.GetByIdAsync("1");

            // With
            Assert.That(result, Is.Not.Null);
            findMock.Verify(f => f.FirstOrDefaultAsync(default), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_Calls_Find_And_ReturnsList()
        {
            // Arrange
            var expectedList = new List<Owner> { new Owner { IdOwner = "1" } };

            var findMock = new Mock<IFindFluent<Owner, Owner>>();

            // Configurar ToListAsync para que devuelva la lista esperada
            findMock
                .Setup(f => f.ToListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedList);

            // También puedes devolver self en ciertas configuraciones (por si se encadenan llamadas)
            findMock
                .SetupGet(f => f.Options)
                .Returns(new FindOptions<Owner, Owner>());

            // Configurar _collection.Find(...) para devolver nuestro findMock
            _collectionMock
                .Setup(c => c.Find(It.IsAny<Expression<Func<Owner, bool>>>(), null))
                .Returns(findMock.Object);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedList));
        }

    }
}