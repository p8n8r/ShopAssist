using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace ShopAssist.ViewModels
{
    public class DisplayableCategory : ViewModelBase
    {
        private ObservableCollection<DisplayableCategory> subcategories;
        public string Name { get; set; }
        public ObservableCollection<DisplayableCategory> Subcategories
        {
            get { return this.subcategories; }
            set { this.subcategories = value; OnPropertyChanged(); }
        }
    }

    internal class CategoryPageViewModel : ViewModelBase
    {
        private ObservableCollection<DisplayableCategory> categories;
        private MainWindowViewModel mainWindowViewModel;
        private DisplayableCategory selectedDisplayableCategory;
        private string searchText, addText;
        public RelayCommand reloadCmd => new RelayCommand(execute => ReloadCategories());
        public RelayCommand selectedItemChangedCmd => new RelayCommand(item => UpdateSelectedCategory(item));
        public RelayCommand searchCmd => new RelayCommand(execute => Search(), canExecute => selectedDisplayableCategory != null);
        public RelayCommand addCmd => new RelayCommand(execute => Add(), canExecute => selectedDisplayableCategory != null);
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
        }

        private void ReloadCategories()
        {
            DisplayableCategory displayableCategory = new DisplayableCategory();
            FillCategories(displayableCategory, this.mainWindowViewModel.Store.Categories.Root);
            this.DisplayableCategories = new ObservableCollection<DisplayableCategory>() { displayableCategory };
        }

        private void FillCategories(DisplayableCategory displayableCategories, TreeNode<Category> node)
        {
            if (node != null)
            {
                displayableCategories.Name = node.Data.Name;

                if (node.Children != null)
                {
                    displayableCategories.Subcategories = new ObservableCollection<DisplayableCategory>();

                    foreach (TreeNode<Category> subnode in node.Children)
                    {
                        DisplayableCategory displayableSubcategory = new DisplayableCategory();
                        FillCategories(displayableSubcategory, subnode);
                        displayableCategories.Subcategories.Add(displayableSubcategory);
                    }
                }
            }
        }

        private void UpdateSelectedCategory(object treeViewItem)
        {
            this.selectedDisplayableCategory = treeViewItem as DisplayableCategory;
        }

        private void Search()
        {
            Category categoryFind = new Category() { Name = this.SearchText };
            TreeNode<Category> categoryNodeFound = this.mainWindowViewModel.Store.Categories.Find(categoryFind);

            if (categoryNodeFound != null)
            {
                TreeViewItem item = null;
                //item.Select
                //this.DisplayableCategories.
                this.SearchText = string.Empty;
            }
            else
            {
                ;//dialogmsg
            }
        }

        private void Add()
        {
            Category categoryCurrent = new Category() { Name = this.selectedDisplayableCategory.Name };
            TreeNode<Category> categoryNodeCurrent = this.mainWindowViewModel.Store.Categories.Find(categoryCurrent);

            if (categoryNodeCurrent != null)
            {
                Category categoryNew = new Category() { Name = this.AddText };
                this.mainWindowViewModel.Store.Categories.AddChild(categoryNodeCurrent, categoryNew);

                this.AddText = string.Empty;
                ReloadCategories(); //Update later more precisely
            }
        }

        private void Remove()
        {
            Category categoryCurrent = new Category() { Name = this.selectedDisplayableCategory.Name };
            TreeNode<Category> categoryNodeCurrent = this.mainWindowViewModel.Store.Categories.Find(categoryCurrent);
            this.mainWindowViewModel.Store.Categories.RemoveChild(categoryNodeCurrent);

            ReloadCategories(); //Update later more precisely
        }
    }
}
