using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System.Collections.Generic;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class TreeNodeTests
    {
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange & Act
            var node = new TreeNode<int>();

            // Assert
            Assert.AreEqual(0, node.Data);
            Assert.IsNull(node.Parent);
            Assert.IsNull(node.Children);
        }

        [TestMethod]
        public void GetHeight_SingleNode_ReturnsOne()
        {
            // Arrange
            var node = new TreeNode<int>();

            // Act
            var height = node.GetHeight();

            // Assert
            Assert.AreEqual(1, height);
        }

        [TestMethod]
        public void GetHeight_TwoLevelTree_ReturnsTwo()
        {
            // Arrange
            var parent = new TreeNode<int>();
            var child = new TreeNode<int> { Parent = parent };
            parent.Children = new List<TreeNode<int>> { child };

            // Act
            var height = child.GetHeight();

            // Assert
            Assert.AreEqual(2, height);
        }

        [TestMethod]
        public void GetHeight_ThreeLevelTree_ReturnsThree()
        {
            // Arrange
            var grandparent = new TreeNode<int>();
            var parent = new TreeNode<int> { Parent = grandparent };
            var child = new TreeNode<int> { Parent = parent };
            grandparent.Children = new List<TreeNode<int>> { parent };
            parent.Children = new List<TreeNode<int>> { child };

            // Act
            var height = child.GetHeight();

            // Assert
            Assert.AreEqual(3, height);
        }
    }
}
