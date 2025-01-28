using ShopAssist.DisplayDialogs;
using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace ShopAssist.ViewModels
{
    public class DisplayableCategory : ViewModelBase
    {
        private ObservableCollection<DisplayableCategory> subcategories;
        public string Name { get; set; }
        public DisplayableCategory ParentCategory { get; set; }
        public ObservableCollection<DisplayableCategory> Subcategories
        {
            get { return this.subcategories; }
            set { this.subcategories = value; OnPropertyChanged(); }
        }
    }

    internal class CategoryPageViewModel : ViewModelBase
    {
        private readonly IDisplayDialog displayDialog;
        private ObservableCollection<DisplayableCategory> categories;
        private MainWindowViewModel mainWindowViewModel;
        private DisplayableCategory rootDisplayableCategory;
        private DisplayableCategory selectedDisplayableCategory;
        private string searchText, addText;
        public RelayCommand reloadCmd => new RelayCommand(execute => ReloadCategories());
        public RelayCommand selectedItemChangedCmd => new RelayCommand(item => UpdateSelectedCategory(item));
        public RelayCommand addCmd => new RelayCommand(execute => Add(),
            canExecute => selectedDisplayableCategory != null && !string.IsNullOrWhiteSpace(this.AddText));
        public RelayCommand removeCmd => new RelayCommand(execute => Remove(), canExecute => selectedDisplayableCategory != null);

        public ObservableCollection<DisplayableCategory> DisplayableCategories
        {
            get { return this.categories; }
            set { this.categories = value; OnPropertyChanged(); }
        }
        public string SearchText
        {
            get { return this.searchText; }
            set { this.searchText = value; OnPropertyChanged(); }
        }
        public string AddText
        {
            get { return this.addText; }
            set { this.addText = value; OnPropertyChanged(); }
        }

        public CategoryPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.displayDialog = this.mainWindowViewModel.displayDialog;
        }

        private void ReloadCategories()
        {
            this.rootDisplayableCategory = new DisplayableCategory();
            FillCategories(this.rootDisplayableCategory, this.mainWindowViewModel.Store.Categories.Root);
            this.DisplayableCategories = new ObservableCollection<DisplayableCategory>() { this.rootDisplayableCategory };
        }

        private void FillCategories(DisplayableCategory displayableCategory, TreeNode<Category> node)
        {
            if (node != null)
            {
                displayableCategory.Name = node.Data.Name;

                if (node.Children != null)
                {
                    displayableCategory.Subcategories = new ObservableCollection<DisplayableCategory>();

                    foreach (TreeNode<Category> subnode in node.Children)
                    {
                        DisplayableCategory displayableSubcategory = new DisplayableCategory() { ParentCategory = displayableCategory };
                        FillCategories(displayableSubcategory, subnode);
                        displayableCategory.Subcategories.Add(displayableSubcategory);
                    }
                }
            }
        }

        public DisplayableCategory FindDisplayableCategory(string name)
        {
            DisplayableCategory displayableCategory = null;
            FindDisplayableCategory(this.rootDisplayableCategory, name, ref displayableCategory);
            return displayableCategory;
        }

        public void FindDisplayableCategory(DisplayableCategory displayableCategory, 
            string name, ref DisplayableCategory displayableCategoryFound)
        {
            if (displayableCategory.Name == name)
            {
                displayableCategoryFound = displayableCategory;
            }
            else
            {
                if (displayableCategory.Subcategories != null)
                {
                    foreach (var displayableSubcategory in displayableCategory.Subcategories)
                    {
                        FindDisplayableCategory(displayableSubcategory, name, ref displayableCategoryFound);

                        if (displayableCategoryFound != null) //Already found? Leave early
                            return;
                    }
                }
            }
        }

        private void UpdateSelectedCategory(object treeViewItem)
        {
            this.selectedDisplayableCategory = treeViewItem as DisplayableCategory;
        }

        private void Add()
        {
            Category categoryCurrent = new Category() { Name = this.selectedDisplayableCategory.Name };
            TreeNode<Category> categoryNodeCurrent = this.mainWindowViewModel.Store.Categories.Find(categoryCurrent);

            if (categoryNodeCurrent != null)
            {
                Category categoryNew = new Category() { Name = this.AddText };
                this.mainWindowViewModel.Store.Categories.AddChild(categoryNodeCurrent, categoryNew);

                DisplayableCategory parentDisplayableCategory = FindDisplayableCategory(categoryNodeCurrent.Data.Name);

                if (parentDisplayableCategory.Subcategories == null)
                    parentDisplayableCategory.Subcategories = new ObservableCollection<DisplayableCategory>();

                parentDisplayableCategory.Subcategories.Add(new DisplayableCategory()
                {
                    Name = this.AddText, 
                    ParentCategory = parentDisplayableCategory 
                });

                this.AddText = string.Empty;
            }
        }

        private void Remove()
        {
            Category categoryCurrent = new Category() { Name = this.selectedDisplayableCategory.Name };
            TreeNode<Category> categoryNodeCurrent = this.mainWindowViewModel.Store.Categories.Find(categoryCurrent);

            DisplayableCategory displayableCategory = FindDisplayableCategory(categoryNodeCurrent.Data.Name);
            DisplayableCategory parentDisplayableCategory = displayableCategory.ParentCategory;

            if (parentDisplayableCategory == null) //The root node?
            {
                this.displayDialog.ShowErrorMessageBox("Cannot remove the \"All\" category.");
            }
            else if (parentDisplayableCategory.Subcategories != null)
            {
                this.mainWindowViewModel.Store.Categories.RemoveChild(categoryNodeCurrent); 
                parentDisplayableCategory.Subcategories.Remove(displayableCategory); //Also removes children
            }
        }
    }
}
