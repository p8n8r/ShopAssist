using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class MembershipTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange & Act
            var membership = new Membership();

            // Assert
            Assert.AreEqual(0, membership.Id);
            Assert.AreEqual(MembershipLevel.No, membership.MembershipLevel);
        }

        [TestMethod]
        public void SetProperties_UpdatesValues()
        {
            // Arrange
            var membership = new Membership();

            // Act
            membership.Id = 1;
            membership.MembershipLevel = MembershipLevel.High;

            // Assert
            Assert.AreEqual(1, membership.Id);
            Assert.AreEqual(MembershipLevel.High, membership.MembershipLevel);
        }
    }
}
