using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAssist;
using System;
using System.Collections.Generic;

namespace ShopAssist.Tests
{
    [TestClass]
    public class ListOperationsTests
    {
        [TestMethod]
        public void QuickSort_SortsListCorrectly()
        {
            // Arrange
            var listQuickSort = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };
            var listNormalSort = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };

            // Act
            ListOperations.QuickSort(listQuickSort);
            listNormalSort.Sort();

            // Assert
            CollectionAssert.AreEqual(listQuickSort, listNormalSort);
        }

        [TestMethod]
        public void QuickSort_AlreadySortedList_RemainsSorted()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            ListOperations.QuickSort(list);

            // Assert
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, list);
        }

        [TestMethod]
        public void QuickSort_ReversedList_SortsCorrectly()
        {
            // Arrange
            var list = new List<int> { 5, 4, 3, 2, 1 };

            // Act
            ListOperations.QuickSort(list);

            // Assert
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            CollectionAssert.AreEqual(expected, list);
        }
    }
}
