using RealEstate.Domain.Entities.Filter;

namespace RealEstate.Tests.Domain
{
    [TestFixture]
    public class PropertySearchParamsTests
    {
        [Test]
        public void DefaultValues_ShouldBeCorrect()
        {
            // Act
            var searchParams = new PropertySearchParams();

            // Assert
            Assert.That(searchParams.Page, Is.EqualTo(1), "El valor por defecto de Page debería ser 1.");
            Assert.That(searchParams.PageSize, Is.EqualTo(20), "El valor por defecto de PageSize debería ser 20.");
            Assert.That(searchParams.OrderByNameDesc, Is.False, "El valor por defecto de OrderByNameDesc debería ser false.");

            Assert.That(searchParams.Name, Is.Null);
            Assert.That(searchParams.Address, Is.Null);
            Assert.That(searchParams.Price, Is.Null);
            Assert.That(searchParams.CodeInternal, Is.Null);
            Assert.That(searchParams.Year, Is.Null);
        }

        [Test]
        public void Should_AssignValuesCorrectly()
        {
            // Arrange & Act
            var searchParams = new PropertySearchParams
            {
                Name = "Juan",
                Address = "Calle 123",
                Price = 100000m,
                CodeInternal = "ABC123",
                Year = 2024,
                Page = 2,
                PageSize = 50,
                OrderByNameDesc = true
            };

            // Assert
            Assert.That(searchParams.Name, Is.EqualTo("Juan"));
            Assert.That(searchParams.Address, Is.EqualTo("Calle 123"));
            Assert.That(searchParams.Price, Is.EqualTo(100000m));
            Assert.That(searchParams.CodeInternal, Is.EqualTo("ABC123"));
            Assert.That(searchParams.Year, Is.EqualTo(2024));
            Assert.That(searchParams.Page, Is.EqualTo(2));
            Assert.That(searchParams.PageSize, Is.EqualTo(50));
            Assert.That(searchParams.OrderByNameDesc, Is.True);
        }
    }
}
