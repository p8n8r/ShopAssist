//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ShopAssist.DisplayDialogs;
//using ShopAssist.Models;
//using ShopAssist.ViewModels;
//using ShopAssist.Views;
//using System.Collections.ObjectModel;

//namespace ShopAssist.ViewModels.Tests
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
//        private MainWindow mainWindow;
//        private MainWindowViewModel mainWindowVm;
//        private CategoryPageViewModel categoryPageVm;

//        [TestInitialize]
//        public void Initialize()
//        {
//            IDisplayDialog fakeDisplayDialog = new FakeDisplayDialog();
//            this.mainWindow = new MainWindow(fakeDisplayDialog);
//            this.mainWindowVm = this.mainWindow.DataContext as MainWindowViewModel;
//            this.categoryPageVm = mainWindow.categoryPage.DataContext as CategoryPageViewModel;
//            this.mainWindowVm.Store = new Store();
//        }

//        [TestMethod]
//        public void ReloadCategories_PopulatesDisplayableCategories()
//        {
//            // Arrange
//            var categoryNode = new TreeNode<Category> { Data = new Category { Name = "Food" } };
//            this.mainWindowVm.Store.Categories.Root = categoryNode;

//            // Act
//            PrivateObject privObj = new PrivateObject(categoryPageVm);
//            privObj.Invoke("ReloadCategories");

//            // Assert
//            Assert.AreEqual(1, categoryPageVm.DisplayableCategories.Count);
//            Assert.AreEqual("Food", categoryPageVm.DisplayableCategories[0].Name);
//        }

//        //[TestMethod]
//        //public void Add_AddsSubcategoryToSelectedCategory()
//        //{
//        //    // Arrange
//        //    var rootCategory = new DisplayableCategory { Name = "All" };
//        //    categoryPageVm.DisplayableCategories = new ObservableCollection<DisplayableCategory> { rootCategory };
//        //    categoryPageVm.SelectedDisplayableCategory = rootCategory;
//        //    categoryPageVm.AddText = "New Category";
//        //    var categoryNode = new TreeNode<Category> { Data = new Category { Name = "All" } };
//        //    mockMainWindowViewModel.Object.Store.Categories.Root = categoryNode;

//        //    // Act
//        //    categoryPageVm.Add();

//        //    // Assert
//        //    Assert.AreEqual(1, rootCategory.Subcategories.Count);
//        //    Assert.AreEqual("New Category", rootCategory.Subcategories[0].Name);
//        //}

//        //[TestMethod]
//        //public void Remove_RemovesSelectedCategory()
//        //{
//        //    // Arrange
//        //    var rootCategory = new DisplayableCategory { Name = "All" };
//        //    var subCategory = new DisplayableCategory { Name = "SubCategory", ParentCategory = rootCategory };
//        //    rootCategory.Subcategories = new ObservableCollection<DisplayableCategory> { subCategory };
//        //    categoryPageVm.DisplayableCategories = new ObservableCollection<DisplayableCategory> { rootCategory };
//        //    categoryPageVm.selectedDisplayableCategory = subCategory;
//        //    var categoryNode = new TreeNode<Category> { Data = new Category { Name = "All" } };
//        //    categoryNode.Children.Add(new TreeNode<Category> { Data = new Category { Name = "SubCategory" } });
//        //    mockMainWindowViewModel.Object.Store.Categories.Root = categoryNode;

//        //    // Act
//        //    categoryPageVm.Remove();

//        //    // Assert
//        //    Assert.AreEqual(0, rootCategory.Subcategories.Count);
//        //}

//        //[TestMethod]
//        //public void Remove_CannotRemoveRootCategory()
//        //{
//        //    // Arrange
//        //    var rootCategory = new DisplayableCategory { Name = "All" };
//        //    categoryPageVm.DisplayableCategories = new ObservableCollection<DisplayableCategory> { rootCategory };
//        //    categoryPageVm.selectedDisplayableCategory = rootCategory;

//        //    // Act
//        //    categoryPageVm.Remove();

//        //    // Assert
//        //    mockDisplayDialog.Verify(d => d.ShowErrorMessageBox("Cannot remove the \"All\" category."));
//        //}
//    }
//}
