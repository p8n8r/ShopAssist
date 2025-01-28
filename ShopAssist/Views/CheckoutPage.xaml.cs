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
    /// Interaction logic for CheckoutPage.xaml
    /// </summary>
    public partial class CheckoutPage : Page
    {
        internal CheckoutPage(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();

            this.DataContext = new CheckoutPageViewModel(mainWindowViewModel);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as CheckoutPageViewModel).reloadCmd.Execute();
        }
    }
}
