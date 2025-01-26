using ShopAssist.DisplayDialogs;
using ShopAssist.Models;
using ShopAssist.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShopAssist.ViewModels
{
    internal class InventoryPageViewModel : ViewModelBase
    {
        private ObservableCollection<Item> inventory;
        private MainWindowViewModel mainWindowViewModel;
        private readonly IDisplayDialog displayDialog;
        private Item selectedItem;
        private string searchText;
        private DataGrid dataGrid;
        public RelayCommand reloadCmd => new RelayCommand(execute => ReloadInventory());
        public RelayCommand searchCmd => new RelayCommand(execute => Search());
        public RelayCommand addCmd => new RelayCommand(execute => IncrementStock(), 
            canExecute => this.SelectedItem != null && this.SelectedItem.Stock < 100);
        public RelayCommand removeCmd => new RelayCommand(execute => DecrementStock(), 
            canExecute => this.SelectedItem != null && this.SelectedItem.Stock > 0);

        public ObservableCollection<Item> Inventory
        {
            get { return this.inventory; }
            set { this.inventory = value; OnPropertyChanged(); }
        }
        public Item SelectedItem
        {
            get { return this.selectedItem; }
            set { this.selectedItem = value; OnPropertyChanged(); }
        }
        public string SearchText
        {
            get { return this.searchText; }
            set { this.searchText = value; OnPropertyChanged(); }
        }

        public InventoryPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            this.displayDialog = this.mainWindowViewModel.displayDialog;
        }

        private void ReloadInventory()
        {
            this.dataGrid = (this.mainWindowViewModel.GetCurrentPage() as InventoryPage).FindName("inventoryDataGrid") as DataGrid;
            this.Inventory = new ObservableCollection<Item>(this.mainWindowViewModel.Store.Inventory.Select(p => p.Value));
        }

        private void Search()
        {
            if (int.TryParse(this.SearchText, out int itemCodeSearch))
            {
                if (this.mainWindowViewModel.Store.Inventory.TryGetValue(itemCodeSearch, out Item itemFound))
                {
                    this.SelectedItem = itemFound;
                    this.dataGrid?.ScrollIntoView(this.SelectedItem);
                    this.SearchText = string.Empty;
                }
                else
                {
                    this.displayDialog.ShowErrorMessageBox($"\"{this.SearchText}\" was not found. Please try again.");
                }
            }
            else
            {
                this.displayDialog.ShowErrorMessageBox($"\"{this.SearchText}\" is not a numerical code. Please try again.");
            }
        }

        private void IncrementStock()
        {
            this.SelectedItem.Stock++;

            //Force refresh of inventory, so inventory can remain plain old CLR objects (pocos).
            CollectionViewSource.GetDefaultView(this.Inventory).Refresh();
        }

        private void DecrementStock()
        {
            this.SelectedItem.Stock--;

            //Force refresh of inventory, so inventory can remain plain old CLR objects (pocos).
            CollectionViewSource.GetDefaultView(this.Inventory).Refresh();
        }
    }
}
