using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.ViewModels
{
    internal class CustomerPageViewModel : ViewModelBase
    {
        private ObservableCollection<Customer> customers;
        private MainWindowViewModel mainWindowViewModel;
        public RelayCommand reloadCmd => new RelayCommand(execute => ReloadCustomers());

        public ObservableCollection<Customer> Customers
        {
            get { return this.customers; }
            set { this.customers = value; OnPropertyChanged(); }
        }

        public CustomerPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        private void ReloadCustomers()
        {
            this.Customers = new ObservableCollection<Customer>(this.mainWindowViewModel.Store.Customers);
        }
    }
}
