using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void CompareTo_SameMembershipId_ReturnsZero()
        {
            // Arrange
            var membership = new Membership { Id = 1 };
            var customer1 = new Customer { Name = "Alice", Membership = membership };
            var customer2 = new Customer { Name = "Bob", Membership = membership };

            // Act
            var result = customer1.CompareTo(customer2);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_DifferentMembershipId_ReturnsPositive()
        {
            // Arrange
            var membership1 = new Membership { Id = 2 };
            var membership2 = new Membership { Id = 1 };
            var customer1 = new Customer { Name = "Alice", Membership = membership1 };
            var customer2 = new Customer { Name = "Bob", Membership = membership2 };

            // Act
            var result = customer1.CompareTo(customer2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareTo_DifferentMembershipId_ReturnsNegative()
        {
            // Arrange
            var membership1 = new Membership { Id = 1 };
            var membership2 = new Membership { Id = 2 };
            var customer1 = new Customer { Name = "Alice", Membership = membership1 };
            var customer2 = new Customer { Name = "Bob", Membership = membership2 };

            // Act
            var result = customer1.CompareTo(customer2);

            // Assert
            Assert.IsTrue(result < 0);
        }

        //[TestMethod]
        //public void CompareTo_NullObject_ThrowsException()
        //{
        //    // Arrange
        //    var membership = new Membership { Id = 1 };
        //    var customer = new Customer { Name = "Alice", Membership = membership };
            
        //    // Act & Assert
        //    Assert.ThrowsException<ArgumentNullException>(() => customer.CompareTo(null));
        //}
    }
}
