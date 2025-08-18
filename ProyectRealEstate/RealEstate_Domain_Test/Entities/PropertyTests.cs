using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Domain.Entities
{
    [TestFixture]
    public class PropertyTests
    {
        [Test]
        public void Property_Should_BeCreated_WithDefaultValues()
        {
            // Act
            var property = new Property();

            // Assert
            Assert.That(property, Is.Not.Null);
            Assert.That(property.Id, Is.Not.Empty); // Se genera un GUID automáticamente
            Assert.That(property.Images, Is.Not.Null);
            Assert.That(property.Images.Count, Is.EqualTo(0));
            Assert.That(property.Traces, Is.Not.Null);
            Assert.That(property.Traces.Count, Is.EqualTo(0));
        }

        [Test]
        public void Property_Should_Assign_Properties_Correctly()
        {
            // Arrange
            var images = new List<PropertyImage>
            {
                new PropertyImage { Enabled = true, File = "https://example.com/img1.jpg" }
            };

            var traces = new List<PropertyTrace>
            {
                new PropertyTrace {  Name = "Compra inicial", Value = 100000, DateSale = new DateTime(2022, 1, 15) }
            };

            // Act
            var property = new Property
            {
                Id = "prop123",
                Name = "Apartamento Centro",
                Address = "Calle 456",
                Price = 200000,
                CodeInternal = "ABC123",
                Year = 2020,
                Images = images,
                Traces = traces
            };

            // Assert
            Assert.That(property.Id, Is.EqualTo("prop123"));
            Assert.That(property.Name, Is.EqualTo("Apartamento Centro"));
            Assert.That(property.Address, Is.EqualTo("Calle 456"));
            Assert.That(property.Price, Is.EqualTo(200000));
            Assert.That(property.CodeInternal, Is.EqualTo("ABC123"));
            Assert.That(property.Year, Is.EqualTo(2020));

            Assert.That(property.Images, Is.Not.Null);
            Assert.That(property.Images.Count, Is.EqualTo(1));
            Assert.That(property.Images[0].File, Is.EqualTo("https://example.com/img1.jpg"));

            Assert.That(property.Traces, Is.Not.Null);
            Assert.That(property.Traces.Count, Is.EqualTo(1));
            Assert.That(property.Traces[0].Name, Is.EqualTo("Compra inicial"));
            Assert.That(property.Traces[0].Value, Is.EqualTo(100000));
        }
    }
}
