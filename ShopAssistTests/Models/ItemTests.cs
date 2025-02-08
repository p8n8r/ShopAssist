using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class ItemTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange & Act
            var item = new Item();

            // Assert
            Assert.IsNull(item.Name);
            Assert.IsNull(item.Category);
            Assert.AreEqual(0, item.Stock);
            Assert.AreEqual(0m, item.Price);
            Assert.AreEqual(0, item.Code);
        }

        [TestMethod]
        public void SetProperties_UpdatesValues()
        {
            // Arrange
            var item = new Item
            {
                // Act
                Name = "Laptop",
                Category = "Electronics",
                Stock = 5,
                Price = 999.99m,
                Code = 12345
            };

            // Assert
            Assert.AreEqual("Laptop", item.Name);
            Assert.AreEqual("Electronics", item.Category);
            Assert.AreEqual(5, item.Stock);
            Assert.AreEqual(999.99m, item.Price);
            Assert.AreEqual(12345, item.Code);
        }
    }
}
