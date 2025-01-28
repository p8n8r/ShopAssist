using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.ViewModels
{
    internal class CheckoutPageViewModel : ViewModelBase
    {
        private MainWindowViewModel mainWindowViewModel;
        private string log;
        private ObservableCollection<Customer> register1Customers, register2Customers, register3Customers;
        public RelayCommand reloadCmd => new RelayCommand(execute => RestartShopping());

        public ObservableCollection<Customer> Register1Customers
        {
            get { return this.register1Customers; }
            set { this.register1Customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Customer> Register2Customers
        {
            get { return this.register2Customers; }
            set { this.register2Customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Customer> Register3Customers
        {
            get { return this.register3Customers; }
            set { this.register3Customers = value; OnPropertyChanged(); }
        }

        public string Log
        {
            get { return log; }
            set { log = value; OnPropertyChanged(); }
        }

        public CheckoutPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        private void RestartShopping()
        {
            //this.rootDisplayableCategory = new DisplayableCategory();
            //FillCategories(this.rootDisplayableCategory, this.mainWindowViewModel.Store.Categories.Root);
            //this.DisplayableCategories = new ObservableCollection<DisplayableCategory>() { this.rootDisplayableCategory };
        }
    }
}
