using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class CategoryTests
    {
        [TestMethod]
        public void Equals_SameName_ReturnsTrue()
        {
            // Arrange
            var category1 = new Category { Name = "Electronics" };
            var category2 = new Category { Name = "Electronics" };

            // Act
            var result = category1.Equals(category2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_DifferentName_ReturnsFalse()
        {
            // Arrange
            var category1 = new Category { Name = "Electronics" };
            var category2 = new Category { Name = "Clothing" };

            // Act
            var result = category1.Equals(category2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_NullObject_ReturnsFalse()
        {
            // Arrange
            var category = new Category { Name = "Electronics" };

            // Act
            var result = category.Equals(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_SameName_ReturnsSameHashCode()
        {
            // Arrange
            var category1 = new Category { Name = "Electronics" };
            var category2 = new Category { Name = "Electronics" };

            // Act
            var hashCode1 = category1.GetHashCode();
            var hashCode2 = category2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void GetHashCode_DifferentName_ReturnsDifferentHashCode()
        {
            // Arrange
            var category1 = new Category { Name = "Electronics" };
            var category2 = new Category { Name = "Clothing" };

            // Act
            var hashCode1 = category1.GetHashCode();
            var hashCode2 = category2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
        }
    }
}
