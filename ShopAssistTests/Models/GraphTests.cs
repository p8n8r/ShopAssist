using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System.Collections.Generic;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void Constructor_InitializesEmptyLists_ReturnsInitializedLists()
        {
            // Arrange & Act
            var graph = new Graph();

            // Assert
            Assert.IsNotNull(graph.Nodes);
            Assert.IsNotNull(graph.Edges);
            Assert.AreEqual(0, graph.Nodes.Count);
            Assert.AreEqual(0, graph.Edges.Count);
        }

        [TestMethod]
        public void AddNode_NodeAddedToList_ReturnsCorrectCount()
        {
            // Arrange
            var graph = new Graph();
            var node = new GraphNode();

            // Act
            graph.Nodes.Add(node);

            // Assert
            Assert.AreEqual(1, graph.Nodes.Count);
            Assert.AreSame(node, graph.Nodes[0]);
        }

        [TestMethod]
        public void AddEdge_EdgeAddedToList_ReturnsCorrectCount()
        {
            // Arrange
            var graph = new Graph();
            var edge = new GraphEdge();

            // Act
            graph.Edges.Add(edge);

            // Assert
            Assert.AreEqual(1, graph.Edges.Count);
            Assert.AreSame(edge, graph.Edges[0]);
        }
    }
}
