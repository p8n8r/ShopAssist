using ShopAssist.DisplayDialogs;
using ShopAssist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopAssist.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly IDisplayDialog displayDialog;
        private MainWindowViewModel mainWindowViewModel;
        public WelcomePage welcomePage;
        public CustomerPage customerPage;
        public InventoryPage inventoryPage;
        public CategoryPage categoryPage;
        public CheckoutPage checkoutPage;
        public DirectionsPage directionsPage;

        public MainWindow(IDisplayDialog displayDialog = null)
        {
            InitializeComponent();

            //Set dialogs as displayable or not
            this.displayDialog = displayDialog ?? new DisplayDialog();
            //Utility.displayDialog = this.displayDialog;

            //Set data context
            this.mainWindowViewModel = new MainWindowViewModel(this, this.displayDialog);
            this.DataContext = this.mainWindowViewModel;

            //Construct pages and viewmodels
            this.welcomePage = new WelcomePage();
            this.customerPage = new CustomerPage(mainWindowViewModel);
            this.inventoryPage = new InventoryPage(mainWindowViewModel);
            this.categoryPage = new CategoryPage(mainWindowViewModel);
            this.checkoutPage = new CheckoutPage(mainWindowViewModel);
            this.directionsPage = new DirectionsPage(mainWindowViewModel);

            //Set initial navigation pages
            this.mainFrame.Navigate(this.welcomePage); //Cannot set in xaml with param
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.mainWindowViewModel.onCloseCmd.Execute();
        }
    }
}
