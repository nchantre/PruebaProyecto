using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Domain.Entities
{
    [TestFixture]
    public class OwnerTests
    {
        [Test]
        public void Owner_Should_BeCreated_WithDefaultValues()
        {
            var owner = new Owner();

            // Assert
            Assert.That(owner, Is.Not.Null);
            Assert.That(owner.Properties, Is.Not.Null); // la lista se inicializa vacía
            Assert.That(owner.Properties.Count, Is.EqualTo(0));
        }

        [Test]
        public void Owner_Should_Assign_Properties_Correctly()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { Id = "p1", Name = "Casa Bonita", Price = 120000 }
            };

            // Act
            var owner = new Owner
            {
                IdOwner = "123abc",
                Name = "Juan Pérez",
                Address = "Calle 123",
                Photo = "https://example.com/foto.jpg",
                Birthday = new DateTime(1990, 5, 20),
                Properties = properties
            };

            // Assert
            Assert.That(owner.IdOwner, Is.EqualTo("123abc"));
            Assert.That(owner.Name, Is.EqualTo("Juan Pérez"));
            Assert.That(owner.Address, Is.EqualTo("Calle 123"));
            Assert.That(owner.Photo, Is.EqualTo("https://example.com/foto.jpg"));
            Assert.That(owner.Birthday, Is.EqualTo(new DateTime(1990, 5, 20)));
            Assert.That(owner.Properties.Count, Is.EqualTo(1));
            Assert.That(owner.Properties[0].Name, Is.EqualTo("Casa Bonita"));
        }
    }
}
