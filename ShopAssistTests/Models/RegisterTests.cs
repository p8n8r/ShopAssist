using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using PommaLabs.Hippie;
using System;
using System.Linq;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class RegisterTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange & Act
            var register = new Register("Register 1");

            // Assert
            Assert.AreEqual("Register 1", register.Name);
            Assert.IsNotNull(register.QueuedCustomers);
            Assert.AreEqual(0, register.QueuedCustomers.Count);
        }

        [TestMethod]
        public void EnterCheckout_AddsCustomerToQueue_UpdatesPositions()
        {
            // Arrange
            var register = new Register("Register 1");
            var customer = new Customer
            {
                Name = "Alice",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.High }
            };
            var queuedCustomer = new QueuedCustomer(customer);

            // Act
            register.EnterCheckout(queuedCustomer);

            // Assert
            Assert.AreEqual(1, register.QueuedCustomers.Count);
            Assert.AreEqual(register, queuedCustomer.Register);
            Assert.AreEqual(1, queuedCustomer.Position);
            Assert.AreEqual(DateTime.Now.Date, queuedCustomer.CheckoutEnteredTime.Date);
        }

        [TestMethod]
        public void LeaveCheckout_RemovesCustomerFromQueue_UpdatesPositions()
        {
            // Arrange
            var register = new Register("Register 1");
            var customer1 = new Customer
            {
                Name = "Alice",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.Medium }
            };
            var customer2 = new Customer
            {
                Name = "Bob",
                Membership = new Membership { Id = 2, MembershipLevel = MembershipLevel.Medium }
            };
            var queuedCustomer1 = new QueuedCustomer(customer1) { CheckoutEnteredTime = DateTime.Now };
            var queuedCustomer2 = new QueuedCustomer(customer2) { CheckoutEnteredTime = DateTime.Now.AddMinutes(1) };
            register.EnterCheckout(queuedCustomer1);
            register.EnterCheckout(queuedCustomer2);

            // Act
            var customerLeft = register.LeaveCheckout();

            // Assert
            Assert.AreEqual(1, register.QueuedCustomers.Count);
            Assert.AreEqual(queuedCustomer1, customerLeft);
            Assert.AreEqual(DateTime.Now.Date, customerLeft.CheckoutEndTime.Date);
        }

        [TestMethod]
        public void AreCustomersWaiting_ReturnsTrueWhenCustomersInQueue()
        {
            // Arrange
            var register = new Register("Register 1");
            var customer = new Customer
            {
                Name = "Alice",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.High }
            };
            var queuedCustomer = new QueuedCustomer(customer);
            register.EnterCheckout(queuedCustomer);

            // Act
            var result = register.AreCustomersWaiting();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SelectLeastBusyRegister_ReturnsRegisterWithFewestCustomers()
        {
            // Arrange
            var register1 = new Register("Register 1");
            var register2 = new Register("Register 2");
            var customer = new Customer
            {
                Name = "Alice",
                Membership = new Membership { Id = 1, MembershipLevel = MembershipLevel.High }
            };
            var queuedCustomer = new QueuedCustomer(customer);
            register1.EnterCheckout(queuedCustomer);

            // Act
            var result = Register.SelectLeastBusyRegister(new[] { register1, register2 });

            // Assert
            Assert.AreEqual(register2, result);
        }
    }
}
