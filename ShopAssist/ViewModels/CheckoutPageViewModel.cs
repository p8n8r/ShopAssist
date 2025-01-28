using ShopAssist.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShopAssist.ViewModels
{
    internal class CheckoutPageViewModel : ViewModelBase
    {
        private const int MIN_INITIAL_CUSTOMERS = 5, MAX_INITIAL_CUSTOMERS = 10;
        private const int REGISTER1 = 0, REGISTER2 = 1, REGISTER3 = 2;
        private MainWindowViewModel mainWindowViewModel;
        private string log;
        private List<Customer> customers, customersReadyToCheckout, customersInCheckout;
        private ObservableCollection<Customer> register1Customers, register2Customers, register3Customers;
        private Random random;
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
            this.Log = string.Empty; //Clear log
            this.Register1Customers = new ObservableCollection<Customer>();
            this.Register2Customers = new ObservableCollection<Customer>();
            this.Register3Customers = new ObservableCollection<Customer>();

            Register[] registers = new Register[3]
            {
                new Register("Register 1"),
                new Register("Register 2"),
                new Register("Register 3")
            };

            this.customers = this.mainWindowViewModel.Store.Customers;
            this.customersReadyToCheckout = this.customers.ToList(); //Make copy of all customers
            
            this.random = new Random();
            this.customersInCheckout = new List<Customer>();

            int initialCustomerCount = this.random.Next(MIN_INITIAL_CUSTOMERS, MAX_INITIAL_CUSTOMERS);
            for (int i = 0; i < initialCustomerCount; i++)
            {
                Customer customer = this.customersReadyToCheckout[random.Next(this.customersReadyToCheckout.Count)];
                this.customersReadyToCheckout.Remove(customer);
                this.customersInCheckout.Add(customer);

                Register registerLeastBusy = Register.SelectLeastBusyRegister(registers);

                int registerNum = Array.IndexOf(registers, registerLeastBusy);
                switch (registerNum)
                {
                    case REGISTER1:
                        this.Register1Customers.Add(customer);
                        break;
                    case REGISTER2:
                        this.Register2Customers.Add(customer);
                        break;
                    case REGISTER3:
                        this.Register3Customers.Add(customer);
                        break;
                }

                registerLeastBusy.EnterCheckout(customer);
            }

            //while (registers.Any(r => r.AreCustomersWaiting()))
            //{


            //    //foreach (Register register in registers)
            //    //{
            //    //    foreach (QueuedCustomer queuedCustomer in register.QueuedCustomers)
            //    //    {

            //    //    }
            //    //}


            //    //...
            //}
        }
    }
}
