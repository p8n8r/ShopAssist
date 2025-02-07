using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System.Collections.Generic;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class GraphNodeTests
    {
        [TestMethod]
        public void Constructor_Parameterless_InitializesProperties()
        {
            // Arrange & Act
            var node = new GraphNode();

            // Assert
            Assert.IsNotNull(node.Edges);
            Assert.AreEqual(int.MaxValue, node.Distance);
            Assert.AreEqual(0, node.X);
            Assert.AreEqual(0, node.Y);
        }

        [TestMethod]
        public void Constructor_Parameterized_InitializesProperties()
        {
            // Arrange
            string name = "A";
            int x = 10;
            int y = 20;

            // Act
            var node = new GraphNode(name, x, y);

            // Assert
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(x, node.X);
            Assert.AreEqual(y, node.Y);
            Assert.IsNotNull(node.Edges);
        }

        [TestMethod]
        public void ToString_ReturnsName()
        {
            // Arrange
            var node = new GraphNode { Name = "A" };

            // Act
            var result = node.ToString();

            // Assert
            Assert.AreEqual("A", result);
        }

        [TestMethod]
        public void Compare_SameDistance_ReturnsZero()
        {
            // Arrange
            var node1 = new GraphNode { Distance = 10 };
            var node2 = new GraphNode { Distance = 10 };
            var comparer = new GraphNode();

            // Act
            var result = comparer.Compare(node1, node2);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Compare_DifferentDistance_ReturnsPositive()
        {
            // Arrange
            var node1 = new GraphNode { Distance = 20 };
            var node2 = new GraphNode { Distance = 10 };
            var comparer = new GraphNode();

            // Act
            var result = comparer.Compare(node1, node2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Compare_DifferentDistance_ReturnsNegative()
        {
            // Arrange
            var node1 = new GraphNode { Distance = 10 };
            var node2 = new GraphNode { Distance = 20 };
            var comparer = new GraphNode();

            // Act
            var result = comparer.Compare(node1, node2);

            // Assert
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareTo_SameDistance_ReturnsZero()
        {
            // Arrange
            var node1 = new GraphNode { Distance = 10 };
            var node2 = new GraphNode { Distance = 10 };

            // Act
            var result = node1.CompareTo(node2);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_DifferentDistance_ReturnsPositive()
        {
            // Arrange
            var node1 = new GraphNode { Distance = 20 };
            var node2 = new GraphNode { Distance = 10 };

            // Act
            var result = node1.CompareTo(node2);

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void CompareTo_DifferentDistance_ReturnsNegative()
        {
            // Arrange
            var node1 = new GraphNode { Distance = 10 };
            var node2 = new GraphNode { Distance = 20 };

            // Act
            var result = node1.CompareTo(node2);

            // Assert
            Assert.IsTrue(result < 0);
        }
    }
}
