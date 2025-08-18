using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using RealEstate.Domain.Exceptions;
using RealEstate.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace RealEstate.Tests.Infrastructure.Repositories
{
    public class GenericRepositoryTests
    {
        private Mock<IMongoCollection<TestEntity>> _mockCollection;
        private GenericRepository<TestEntity> _repository;

        public class TestEntity
        {
            public ObjectId Id { get; set; }
            public string Name { get; set; } = default!;
        }

        [SetUp]
        public void Setup()
        {
            _mockCollection = new Mock<IMongoCollection<TestEntity>>();
            _repository = new GenericRepository<TestEntity>(_mockCollection.Object);
        }
        [Test]
        public async Task GetAllAsync_ReturnsEntities_WithCursor()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Test" }
            };

            // Mock del cursor
            var mockCursor = new Mock<IAsyncCursor<TestEntity>>();
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            mockCursor.SetupGet(c => c.Current).Returns(data);

            // Mock de la colección - Usando FindAsync en lugar de FindSync
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<FindOptions<TestEntity, TestEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Test"));
        }




        [Test]
        public async Task GetByIdAsync_Found_ReturnsEntity()
        {
            // Arrange
            var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Entity1" };
            var id = entity.Id.ToString();

            // Mock del cursor
            var mockCursor = new Mock<IAsyncCursor<TestEntity>>();
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            mockCursor.SetupGet(c => c.Current).Returns(new List<TestEntity> { entity });

            // Configuración alternativa que evita mockear métodos de extensión
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<FindOptions<TestEntity, TestEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo("Entity1"));
        }
        [Test]
        public async Task GetByIdAsync_NotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var id = ObjectId.GenerateNewId().ToString();
            const string expectedEntityName = "TestEntity"; // Nombre de tu entidad

            // Configuración del mock
            var emptyCursor = new Mock<IAsyncCursor<TestEntity>>();
            emptyCursor.Setup(_ => _.Current).Returns(new List<TestEntity>());
            emptyCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true)
                       .ReturnsAsync(false);

            _mockCollection.Setup(x => x.FindAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<FindOptions<TestEntity, TestEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyCursor.Object);

            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() => _repository.GetByIdAsync(id));
         
            Assert.That(ex.Message, Does.Contain(expectedEntityName));
            Assert.That(ex.Message, Does.Contain(id));
        }



        [Test]
        public async Task AddAsync_CallsInsertOne()
        {
            var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Name = "New" };

            _mockCollection.Setup(c => c.InsertOneAsync(entity, null, It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            await _repository.AddAsync(entity);

            _mockCollection.Verify(c => c.InsertOneAsync(entity, null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsReplaceOne()
        {
            var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Update" };
            var id = entity.Id.ToString();

            _mockCollection.Setup(c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    entity,
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ReplaceOneResult>(r => r.MatchedCount == 1))
                .Verifiable();

            await _repository.UpdateAsync(id, entity);

            _mockCollection.Verify(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<TestEntity>>(),
                                                           entity,
                                                           It.IsAny<ReplaceOptions>(),
                                                           It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_CallsDeleteOne()
        {
            var id = ObjectId.GenerateNewId().ToString();

            _mockCollection.Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<DeleteResult>(r => r.DeletedCount == 1))
                .Verifiable();

            await _repository.DeleteAsync(id);

            _mockCollection.Verify(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<TestEntity>>(),
                                                          It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
