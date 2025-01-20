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
            get { return searchText; }
            set { searchText = value; OnPropertyChanged(); }
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
                }
                else
                {
                    displayDialog.ShowErrorMessageBox($"\"{this.SearchText}\" was not found. Please try again.");
                }
            }
            else
            {
                displayDialog.ShowErrorMessageBox($"\"{this.SearchText}\" is not a numerical code. Please try again.");
            }
        }
    }
}
