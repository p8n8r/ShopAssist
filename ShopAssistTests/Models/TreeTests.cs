using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist.Models;
using System.Collections.Generic;

namespace ShopAssist.Models.Tests
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void RestoreParents_RestoresParentReferences()
        {
            // Arrange
            var root = new TreeNode<int> { Data = 1 };
            var child = new TreeNode<int> { Data = 2 };
            root.Children = new List<TreeNode<int>> { child };
            var tree = new Tree<int> { Root = root };

            // Act
            tree.RestoreParents();

            // Assert
            Assert.AreEqual(root, child.Parent);
        }

        [TestMethod]
        public void Has_DataExistsInTree_ReturnsTrue()
        {
            // Arrange
            var root = new TreeNode<int> { Data = 1 };
            var child = new TreeNode<int> { Data = 2 };
            root.Children = new List<TreeNode<int>> { child };
            var tree = new Tree<int> { Root = root };

            // Act
            var result = tree.Has(2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Has_DataDoesNotExistInTree_ReturnsFalse()
        {
            // Arrange
            var root = new TreeNode<int> { Data = 1 };
            var tree = new Tree<int> { Root = root };

            // Act
            var result = tree.Has(2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Find_DataExistsInTree_ReturnsCorrectNode()
        {
            // Arrange
            var root = new TreeNode<int> { Data = 1 };
            var child = new TreeNode<int> { Data = 2 };
            root.Children = new List<TreeNode<int>> { child };
            var tree = new Tree<int> { Root = root };

            // Act
            var result = tree.Find(2);

            // Assert
            Assert.AreEqual(child, result);
        }

        [TestMethod]
        public void Find_DataDoesNotExistInTree_ReturnsNull()
        {
            // Arrange
            var root = new TreeNode<int> { Data = 1 };
            var tree = new Tree<int> { Root = root };

            // Act
            var result = tree.Find(2);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddChild_AddsChildToNode()
        {
            // Arrange
            var root = new TreeNode<int> { Data = 1 };
            var tree = new Tree<int> { Root = root };

            // Act
            tree.AddChild(root, 2);

            // Assert
            Assert.AreEqual(1, root.Children.Count);
            Assert.AreEqual(2, root.Children[0].Data);
        }

        [TestMethod]
        public void RemoveChild_RemovesChildFromNode()
        {
            // Arrange
            var root = new TreeNode<int> { Data = 1 };
            var child = new TreeNode<int> { Data = 2, Parent = root };
            root.Children = new List<TreeNode<int>> { child };
            var tree = new Tree<int> { Root = root };

            // Act
            tree.RemoveChild(child);

            // Assert
            Assert.AreEqual(0, root.Children.Count);
        }
    }
}
