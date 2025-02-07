using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShopAssist.Tests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void Sort_SortsObservableCollection()
        {
            // Arrange
            var collection = new ObservableCollection<int> { 3, 1, 2 };

            // Act
            collection.Sort();

            // Assert
            Assert.AreEqual(1, collection[0]);
            Assert.AreEqual(2, collection[1]);
            Assert.AreEqual(3, collection[2]);
        }

        [TestMethod]
        public void FindVisualChildren_FindsAllChildren()
        {
            // Arrange
            var parent = new StackPanel();
            var child1 = new Button();
            var child2 = new TextBox();
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            // Act
            var children = Utility.FindVisualChildren<UIElement>(parent).ToList();

            // Assert
            Assert.AreEqual(2, children.Count);
            Assert.IsTrue(children.Contains(child1));
            Assert.IsTrue(children.Contains(child2));
        }

        [TestMethod]
        public void FindVisualChildren_NoChildren_ReturnsEmpty()
        {
            // Arrange
            var parent = new StackPanel();

            // Act
            var children = Utility.FindVisualChildren<UIElement>(parent).ToList();

            // Assert
            Assert.AreEqual(0, children.Count);
        }
    }
}
