using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System.Collections.Generic;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class StoreTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange & Act
            var store = new Store();

            // Assert
            Assert.IsNotNull(store.Customers);
            Assert.IsNotNull(store.Inventory);
            Assert.IsNotNull(store.Categories);
            Assert.IsNotNull(store.DirectionsGraph);
            Assert.IsNull(store.Items); // Since Items are only used for import/export
            Assert.AreEqual(0, store.Customers.Count);
            Assert.AreEqual(0, store.Inventory.Count);
        }
    }
}
