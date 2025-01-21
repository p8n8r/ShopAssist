using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ShopAssist.ViewModels
{
    public class DisplayableCategory : ViewModelBase
    {
        private ObservableCollection<Category> subcategories;
        public string Name { get; set; }
        public ObservableCollection<Category> Subcategories
        {
            get { return this.subcategories; }
            set { this.subcategories = value; OnPropertyChanged(); }
        }
    }

    internal class CategoryPageViewModel : ViewModelBase
    {
        private ObservableCollection<DisplayableCategory> categories;
        private MainWindowViewModel mainWindowViewModel;
        public RelayCommand reloadCmd => new RelayCommand(execute => ReloadCategories());

        public ObservableCollection<DisplayableCategory> DisplayableCategories
        {
            get { return this.categories; }
            set { this.categories = value; OnPropertyChanged(); }
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
                displayableCategories.Subcategories = new ObservableCollection<Category>();

                if (node.Children != null)
                {
                    foreach (TreeNode<Category> subnode in node.Children)
                    {
                        DisplayableCategory displayableSubcategory = new DisplayableCategory();
                        FillCategories(displayableSubcategory, subnode);
                    }
                }
            }
        }
    }
}
