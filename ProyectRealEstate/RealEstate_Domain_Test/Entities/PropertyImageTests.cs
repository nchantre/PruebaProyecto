using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Domain.Entities
{
    [TestFixture]
    public class PropertyImageTests
    {
        [Test]
        public void PropertyImage_Should_BeCreated_WithDefaultValues()
        {
            // Act
            var image = new PropertyImage();

            // Assert
            Assert.That(image, Is.Not.Null);
            Assert.That(image.File, Is.Null); // default! no inicializa valor
            Assert.That(image.Enabled, Is.True); // por defecto está habilitada
        }

        [Test]
        public void PropertyImage_Should_Assign_Properties_Correctly()
        {
            // Act
            var image = new PropertyImage
            {
                File = "https://example.com/photo.jpg",
                Enabled = false
            };

            // Assert
            Assert.That(image.File, Is.EqualTo("https://example.com/photo.jpg"));
            Assert.That(image.Enabled, Is.False);
        }
    }
}
