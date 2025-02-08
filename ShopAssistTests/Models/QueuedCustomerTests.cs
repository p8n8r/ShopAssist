using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class QueuedCustomerTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "Peyton",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.High }
            };

            // Act
            var queuedCustomer = new QueuedCustomer(customer);

            // Assert
            Assert.AreEqual(customer.Name, queuedCustomer.Name);
            Assert.AreEqual(customer.Membership, queuedCustomer.Membership);
        }

        [TestMethod]
        public void MemberPriority_ReturnsCorrectFormat()
        {
            // Arrange
            var customer = new Customer
            {
                Name = "Peyton",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.High }
            };
            var queuedCustomer = new QueuedCustomer(customer);

            // Act
            var result = queuedCustomer.MemberPriority;

            // Assert
            Assert.AreEqual("Peyton (High Priority)", result);
        }

        [TestMethod]
        public void CompareTo_HigherPriority_ReturnsPositive()
        {
            // Arrange
            var customer1 = new Customer
            {
                Name = "Alice",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.Medium }
            };
            var customer2 = new Customer
            {
                Name = "Bob",
                Membership = new Membership { Id = 2, MembershipLevel = MembershipLevel.High }
            };
            var queuedCustomer1 = new QueuedCustomer(customer1) { CheckoutEnteredTime = DateTime.Now };
            var queuedCustomer2 = new QueuedCustomer(customer2) { CheckoutEnteredTime = DateTime.Now };

            // Act
            var result = queuedCustomer1.CompareTo(queuedCustomer2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareTo_SamePriorityEarlierTime_ReturnsNegative()
        {
            // Arrange
            var customer1 = new Customer
            {
                Name = "Alice",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.High }
            };
            var customer2 = new Customer
            {
                Name = "Bob",
                Membership = new Membership { Id = 2, MembershipLevel = MembershipLevel.High }
            };
            var queuedCustomer1 = new QueuedCustomer(customer1) { CheckoutEnteredTime = DateTime.Now };
            var queuedCustomer2 = new QueuedCustomer(customer2) { CheckoutEnteredTime = DateTime.Now.AddMinutes(1) };

            // Act
            var result = queuedCustomer1.CompareTo(queuedCustomer2);

            // Assert
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareTo_SamePriorityLaterTime_ReturnsPositive()
        {
            // Arrange
            var customer1 = new Customer
            {
                Name = "Alice",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.High }
            };
            var customer2 = new Customer
            {
                Name = "Bob",
                Membership = new Membership { Id = 2, MembershipLevel = MembershipLevel.High }
            };
            var queuedCustomer1 = new QueuedCustomer(customer1) { CheckoutEnteredTime = DateTime.Now.AddMinutes(1) };
            var queuedCustomer2 = new QueuedCustomer(customer2) { CheckoutEnteredTime = DateTime.Now };

            // Act
            var result = queuedCustomer1.CompareTo(queuedCustomer2);

            // Assert
            Assert.IsTrue(result > 0);
        }
    }
}
