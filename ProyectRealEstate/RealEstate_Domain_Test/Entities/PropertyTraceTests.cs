using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Domain.Entities
{
    [TestFixture]
    public class PropertyTraceTests
    {
        [Test]
        public void PropertyTrace_Should_BeCreated_WithDefaultValues()
        {
            // Act
            var trace = new PropertyTrace();

            // Assert
            Assert.That(trace, Is.Not.Null);
            Assert.That(trace.Name, Is.Null); // default! → sin inicializar
            Assert.That(trace.DateSale, Is.EqualTo(default(DateTime))); // fecha no inicializada
            Assert.That(trace.Value, Is.EqualTo(0));
            Assert.That(trace.Tax, Is.EqualTo(0));
        }

        [Test]
        public void PropertyTrace_Should_Assign_Properties_Correctly()
        {
            // Arrange
            var date = new DateTime(2023, 12, 1);

            // Act
            var trace = new PropertyTrace
            {
                DateSale = date,
                Name = "Venta inicial",
                Value = 150000,
                Tax = 5000
            };

            // Assert
            Assert.That(trace.DateSale, Is.EqualTo(date));
            Assert.That(trace.Name, Is.EqualTo("Venta inicial"));
            Assert.That(trace.Value, Is.EqualTo(150000));
            Assert.That(trace.Tax, Is.EqualTo(5000));
        }
    }
}
