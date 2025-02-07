//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ShopAssist.DisplayDialogs;
//using ShopAssist.Models;
//using ShopAssist.ViewModels;
//using System.Collections.ObjectModel;

//namespace ShopAssist.Tests
//{
//    [TestClass]
//    public class DisplayableCategoryTests
//    {
//        [TestMethod]
//        public void Constructor_InitializesProperties()
//        {
//            // Arrange & Act
//            var category = new DisplayableCategory();

//            // Assert
//            Assert.AreEqual(true, category.IsExpanded);
//            Assert.IsNull(category.Name);
//            Assert.IsNull(category.ParentCategory);
//            Assert.IsNull(category.Subcategories);
//        }
//    }

//    [TestClass]
//    public class CategoryPageViewModelTests
//    {
//        private Mock<IDisplayDialog> mockDisplayDialog;
//        private Mock<MainWindowViewModel> mockMainWindowViewModel;
//        private CategoryPageViewModel viewModel;

//        [TestInitialize]
//        public void Initialize()
//        {
//            mockDisplayDialog = new Mock<IDisplayDialog>();
//            mockMainWindowViewModel = new Mock<MainWindowViewModel>();
//            mockMainWindowViewModel.Setup(m => m.displayDialog).Returns(mockDisplayDialog.Object);
//            mockMainWindowViewModel.Setup(m => m.Store).Returns(new Store());
//            viewModel = new CategoryPageViewModel(mockMainWindowViewModel.Object);
//        }

//        [TestMethod]
//        public void ReloadCategories_PopulatesDisplayableCategories()
//        {
//            // Arrange
//            var categoryNode = new TreeNode<Category> { Data = new Category { Name = "Electronics" } };
//            mockMainWindowViewModel.Object.Store.Categories.Root = categoryNode;

//            // Act
//            viewModel.ReloadCategories();

//            // Assert
//            Assert.AreEqual(1, viewModel.DisplayableCategories.Count);
//            Assert.AreEqual("Electronics", viewModel.DisplayableCategories[0].Name);
//        }

//        [TestMethod]
//        public void Add_AddsSubcategoryToSelectedCategory()
//        {
//            // Arrange
//            var rootCategory = new DisplayableCategory { Name = "All" };
//            viewModel.DisplayableCategories = new ObservableCollection<DisplayableCategory> { rootCategory };
//            viewModel.selectedDisplayableCategory = rootCategory;
//            viewModel.AddText = "New Category";
//            var categoryNode = new TreeNode<Category> { Data = new Category { Name = "All" } };
//            mockMainWindowViewModel.Object.Store.Categories.Root = categoryNode;

//            // Act
//            viewModel.Add();

//            // Assert
//            Assert.AreEqual(1, rootCategory.Subcategories.Count);
//            Assert.AreEqual("New Category", rootCategory.Subcategories[0].Name);
//        }

//        [TestMethod]
//        public void Remove_RemovesSelectedCategory()
//        {
//            // Arrange
//            var rootCategory = new DisplayableCategory { Name = "All" };
//            var subCategory = new DisplayableCategory { Name = "SubCategory", ParentCategory = rootCategory };
//            rootCategory.Subcategories = new ObservableCollection<DisplayableCategory> { subCategory };
//            viewModel.DisplayableCategories = new ObservableCollection<DisplayableCategory> { rootCategory };
//            viewModel.selectedDisplayableCategory = subCategory;
//            var categoryNode = new TreeNode<Category> { Data = new Category { Name = "All" } };
//            categoryNode.Children.Add(new TreeNode<Category> { Data = new Category { Name = "SubCategory" } });
//            mockMainWindowViewModel.Object.Store.Categories.Root = categoryNode;

//            // Act
//            viewModel.Remove();

//            // Assert
//            Assert.AreEqual(0, rootCategory.Subcategories.Count);
//        }

//        [TestMethod]
//        public void Remove_CannotRemoveRootCategory()
//        {
//            // Arrange
//            var rootCategory = new DisplayableCategory { Name = "All" };
//            viewModel.DisplayableCategories = new ObservableCollection<DisplayableCategory> { rootCategory };
//            viewModel.selectedDisplayableCategory = rootCategory;

//            // Act
//            viewModel.Remove();

//            // Assert
//            mockDisplayDialog.Verify(d => d.ShowErrorMessageBox("Cannot remove the \"All\" category."));
//        }
//    }
//}
