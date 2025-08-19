using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using RealEstate.Domain.Exceptions;
using RealEstate.Domain.Specifications;
using RealEstate.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace RealEstate.Tests.Infrastructure.Repositories
{
    public class GenericRepositoryTests
    {
        private Mock<IMongoCollection<TestEntity>> _mockCollection;
        private TestableGenericRepository<TestEntity> _repository;

        public class TestEntity
        {
            public ObjectId Id { get; set; }
            public string Name { get; set; } = default!;
        }

        // Clase testable con la misma restricción where T : class
        public class TestableGenericRepository<T> : GenericRepository<T> where T : class
        {
            private readonly IQueryable<T> _testData;

            public TestableGenericRepository(IMongoCollection<T> collection, IQueryable<T> testData)
                : base(collection)
            {
                _testData = testData;
            }

            // Mantén el mismo modificador de acceso (protected)
            protected override IQueryable<T> GetQueryable()
            {
                return _testData;
            }
        }

        // Fake Specification sin proyección
        public class FakeSpecification : ISpecification<TestEntity>
        {
            public Expression<Func<TestEntity, bool>> Criteria => e => e.Name == "Uno";
            public List<Expression<Func<TestEntity, object>>> Includes { get; } = new();
            public List<string> IncludeStrings { get; } = new();
            public Expression<Func<TestEntity, object>> OrderBy => null;
            public Expression<Func<TestEntity, object>> OrderByDescending => null;
            public int? Take => null;
            public int? Skip => null;
            public bool IsPagingEnabled => false;
        }

        // Fake Specification con proyección
        public class FakeProjectionSpec : ISpecification<TestEntity, string>
        {
            public Expression<Func<TestEntity, string>> Selector => e => e.Name;
            public Expression<Func<TestEntity, bool>> Criteria => e => true;
            public List<Expression<Func<TestEntity, object>>> Includes { get; } = new();
            public List<string> IncludeStrings { get; } = new();
            public Expression<Func<TestEntity, object>> OrderBy => null;
            public Expression<Func<TestEntity, object>> OrderByDescending => null;
            public int? Take => null;
            public int? Skip => null;
            public bool IsPagingEnabled => false;
        }

        [SetUp]
        public void Setup()
        {
            var data = new List<TestEntity>
            {
                new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Uno" },
                new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Dos" }
            };

            _mockCollection = new Mock<IMongoCollection<TestEntity>>();
            _repository = new TestableGenericRepository<TestEntity>(_mockCollection.Object, data.AsQueryable());
        }

        // Tests existentes (mantener todos)
        [Test]
        public async Task GetAllAsync_ReturnsEntities_WithCursor()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Test" }
            };

            var mockCursor = new Mock<IAsyncCursor<TestEntity>>();
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            mockCursor.SetupGet(c => c.Current).Returns(data);

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

            var mockCursor = new Mock<IAsyncCursor<TestEntity>>();
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            mockCursor.SetupGet(c => c.Current).Returns(new List<TestEntity> { entity });

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
            const string expectedEntityName = "TestEntity";

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

            // Act & Assert
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(() => _repository.GetByIdAsync(id));
            Assert.That(ex.Message, Does.Contain(expectedEntityName));
            Assert.That(ex.Message, Does.Contain(id));
        }

        [Test]
        public async Task AddAsync_CallsInsertOne()
        {
            // Arrange
            var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Name = "New" };

            _mockCollection.Setup(c => c.InsertOneAsync(entity, null, It.IsAny<CancellationToken>()))
                           .Returns(Task.CompletedTask)
                           .Verifiable();

            // Act
            await _repository.AddAsync(entity);

            // Assert
            _mockCollection.Verify(c => c.InsertOneAsync(entity, null, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_CallsReplaceOne()
        {
            // Arrange
            var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Update" };
            var id = entity.Id.ToString();

            _mockCollection.Setup(c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    entity,
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<ReplaceOneResult>(r => r.MatchedCount == 1))
                .Verifiable();

            // Act
            await _repository.UpdateAsync(id, entity);

            // Assert
            _mockCollection.Verify(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<TestEntity>>(),
                                                           entity,
                                                           It.IsAny<ReplaceOptions>(),
                                                           It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_CallsDeleteOne()
        {
            // Arrange
            var id = ObjectId.GenerateNewId().ToString();

            _mockCollection.Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<DeleteResult>(r => r.DeletedCount == 1))
                .Verifiable();

            // Act
            await _repository.DeleteAsync(id);

            // Assert
            _mockCollection.Verify(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<TestEntity>>(),
                                                          It.IsAny<CancellationToken>()), Times.Once);
        }

        // NUEVOS TESTS PARA SPECIFICATION

        [Test]
        public void ProjectionSpecification_Works_Correctly()
        {
            // Arrange
            var spec = new FakeProjectionSpec();
            var data = new List<TestEntity>
    {
        new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Uno" },
        new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Dos" }
    };

            // Test manual de la proyección
            var queryable = data.AsQueryable();
            var projectedQuery = queryable.Where(spec.Criteria).Select(spec.Selector);
            var result = projectedQuery.ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Does.Contain("Uno"));
            Assert.That(result, Does.Contain("Dos"));
        }

        [Test]
        public async Task GetAsync_WithProjectionSpec_ReturnsProjectedData()
        {
            // Arrange
            var spec = new FakeProjectionSpec();
            var projectedData = new List<string> { "Uno", "Dos" };

            var mockCursor = new Mock<IAsyncCursor<string>>();
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            mockCursor.SetupGet(c => c.Current).Returns(projectedData);

            // Mock de FindAsync (ahora sí se usará este método)
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<FindOptions<TestEntity, string>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAsync(spec);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Does.Contain("Uno"));
            Assert.That(result, Does.Contain("Dos"));
        }

        [Test]
        public async Task CountAsync_WithSpec_ReturnsCorrectCount()
        {
            // Arrange
            var spec = new FakeSpecification();
            var filteredData = new List<TestEntity>
            {
                new TestEntity { Id = ObjectId.GenerateNewId(), Name = "Uno" }
            };

            // Mock específico para CountAsync
            var mockQueryable = filteredData.AsQueryable();

            // Crear un mock del async cursor para count
            var mockCursor = new Mock<IAsyncCursor<TestEntity>>();
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            mockCursor.SetupGet(c => c.Current).Returns(filteredData);

            // Mock para FindAsync (usado por ToListAsync)
            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<FindOptions<TestEntity, TestEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Mock específico para CountDocumentsAsync
            _mockCollection
                .Setup(c => c.CountDocumentsAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<CountOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var count = await _repository.CountAsync(spec);

            // Assert
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAsync_WithSpec_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var spec = new FakeSpecification();
            var emptyData = new List<TestEntity>();

            var mockCursor = new Mock<IAsyncCursor<TestEntity>>();
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            mockCursor.SetupGet(c => c.Current).Returns(emptyData);

            _mockCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<TestEntity>>(),
                    It.IsAny<FindOptions<TestEntity, TestEntity>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAsync(spec);

            // Assert
            Assert.That(result, Is.Empty);
        }

    }
}