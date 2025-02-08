using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class GraphEdgeTests
    {
        [TestMethod]
        public void Constructor_Parameterless_InitializesProperties()
        {
            // Arrange & Act
            var edge = new GraphEdge();

            // Assert
            Assert.IsNull(edge.From);
            Assert.IsNull(edge.To);
            Assert.AreEqual(0, edge.Weight);
            Assert.AreEqual(0, edge.CenterX);
            Assert.AreEqual(0, edge.CenterY);
        }

        [TestMethod]
        public void Constructor_Parameterized_InitializesProperties()
        {
            // Arrange
            var fromNode = new GraphNode { Name = "A" };
            var toNode = new GraphNode { Name = "B" };
            int weight = 10;

            // Act
            var edge = new GraphEdge(fromNode, toNode, weight);

            // Assert
            Assert.AreSame(fromNode, edge.From);
            Assert.AreSame(toNode, edge.To);
            Assert.AreEqual(weight, edge.Weight);
        }

        [TestMethod]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var fromNode = new GraphNode { Name = "A" };
            var toNode = new GraphNode { Name = "B" };
            var edge = new GraphEdge(fromNode, toNode, 10);

            // Act
            var result = edge.ToString();

            // Assert
            Assert.AreEqual("A -> B", result);
        }
    }
}
