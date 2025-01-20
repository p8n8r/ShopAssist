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
        internal MainWindowViewModel mainWindowViewModel;
        internal WelcomePage welcomePage;

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
            //this.loginPage = new LogInPage(mainWindowViewModel);
            //this.browsePage = new BrowsePage(mainWindowViewModel);
            //this.returnsPage = new ReturnsPage(mainWindowViewModel);
            //this.donatePage = new DonatePage(mainWindowViewModel);

            //Set initial navigation pages
            this.mainFrame.Navigate(this.welcomePage); //Cannot set in xaml with param
            //BrowsePageViewModel browsePageViewModel = this.browsePage.DataContext as BrowsePageViewModel;
            //this.browsePage.mediaTableFrame.Navigate(browsePageViewModel.browseBooksPage);
            //DonatePageViewModel donatePageViewModel = this.donatePage.DataContext as DonatePageViewModel;
            //this.donatePage.mediaTableFrame.Navigate(donatePageViewModel.bookDonationPage);
        }

        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    this.mainWindowViewModel.onCloseCmd.Execute();
        //}
    }
}
