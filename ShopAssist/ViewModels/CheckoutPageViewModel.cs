using Microsoft.Win32;
using ShopAssist.Models;
using ShopAssist.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShopAssist.ViewModels
{
    public class CheckoutPageViewModel : ViewModelBase
    {
        private const int MIN_INITIAL_CUSTOMERS = 5, MAX_INITIAL_CUSTOMERS = 10;
        private const int REGISTER1 = 0, REGISTER2 = 1, REGISTER3 = 2;
        private MainWindowViewModel mainWindowViewModel;
        private CheckoutPage checkoutPage;
        private string log;
        private Register[] registers;
        private List<Customer> customers, customersReadyToCheckout, customersInCheckout;
        private ObservableCollection<QueuedCustomer> register1Customers, register2Customers, register3Customers;
        private Random random;
        public RelayCommand reloadCmd => new RelayCommand(execute => RestartShopping());

        public ObservableCollection<QueuedCustomer> Register1Customers
        {
            get { return this.register1Customers; }
            set { this.register1Customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<QueuedCustomer> Register2Customers
        {
            get { return this.register2Customers; }
            set { this.register2Customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<QueuedCustomer> Register3Customers
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

        private async void RestartShopping()
        {
            this.checkoutPage = this.mainWindowViewModel.GetCurrentPage() as CheckoutPage;

            this.Log = string.Empty; //Clear log
            this.Register1Customers = new ObservableCollection<QueuedCustomer>();
            this.Register2Customers = new ObservableCollection<QueuedCustomer>();
            this.Register3Customers = new ObservableCollection<QueuedCustomer>();

            this.registers = new Register[3]
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
                await CustomerEntersCheckoutAsync();
            }

            while (this.mainWindowViewModel.GetCurrentPage() is CheckoutPage)
            {
                await CustomerEntersCheckoutAsync();
                await Task.Delay(random.Next(1000, 2000));
                await CustomerExitsCheckoutAsync();
                await Task.Delay(random.Next(1000, 2000));
            }
        }

        private void AddCustomerToRegister(QueuedCustomer customer, Register register)
        {
            int registerNum = Array.IndexOf(registers, register);

            this.checkoutPage.Dispatcher.Invoke(() =>
            {
                switch (registerNum)
                {
                    case REGISTER1:
                        this.Register1Customers.Add(customer);
                        this.Register1Customers.Sort();
                        break;
                    case REGISTER2:
                        this.Register2Customers.Add(customer);
                        this.Register2Customers.Sort();
                        break;
                    case REGISTER3:
                        this.Register3Customers.Add(customer);
                        this.Register3Customers.Sort();
                        break;
                }
            });
        }

        private void RemoveCustomerFromRegister(QueuedCustomer customer, Register register)
        {
            int registerNum = Array.IndexOf(registers, register);

            this.checkoutPage.Dispatcher.Invoke(() =>
            {
                switch (registerNum)
                {
                    case REGISTER1:
                        this.Register1Customers.Remove(customer);
                        this.Register1Customers.Sort();
                        break;
                    case REGISTER2:
                        this.Register2Customers.Remove(customer);
                        this.Register2Customers.Sort();
                        break;
                    case REGISTER3:
                        this.Register3Customers.Remove(customer);
                        this.Register3Customers.Sort();
                        break;
                }
            });
        }

        private async Task CustomerEntersCheckoutAsync()
        {
            if (this.customersReadyToCheckout.Count > 0)
            {
                //Choose random customer to start business at least busy register
                Customer customer = this.customersReadyToCheckout[random.Next(this.customersReadyToCheckout.Count)];
                this.customersReadyToCheckout.Remove(customer);
                this.customersInCheckout.Add(customer);

                Register registerLeastBusy = Register.SelectLeastBusyRegister(registers);
                QueuedCustomer queuedCustomer = new QueuedCustomer(customer);
                AddCustomerToRegister(queuedCustomer, registerLeastBusy);
                registerLeastBusy.EnterCheckout(queuedCustomer);

                //Log it!
                string message = $"{queuedCustomer.MemberPriority} has entered {registerLeastBusy.Name}.";
                LogMessage(message);
            }
        }

        private async Task CustomerExitsCheckoutAsync()
        {
            if (this.registers.Any(r => r.QueuedCustomers.Count > 0))
            {
                //Choose random register to have finished business with customer
                Register register = registers[random.Next(registers.Count())];
                QueuedCustomer queuedCustomer = register.LeaveCheckout();

                if (queuedCustomer != null)
                {
                    Customer customer = this.customersInCheckout.First(c => c.Membership.Id == queuedCustomer.Membership.Id);
                    this.customersInCheckout.Remove(customer); 
                    this.customersReadyToCheckout.Add(customer);
                    RemoveCustomerFromRegister(queuedCustomer, register);

                    TimeSpan timeWaited = queuedCustomer.CheckoutEndTime - queuedCustomer.CheckoutEnteredTime;

                    //Log it!
                    string message = $"{queuedCustomer.MemberPriority} waited for {(int)timeWaited.TotalSeconds} seconds and has left {register.Name}.";
                    LogMessage(message);
                }
            }
        }

        private void LogMessage(string message)
        {
            this.Log += message + "\n";
        }
    }
}
