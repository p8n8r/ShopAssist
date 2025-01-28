using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.ViewModels
{
    internal class CheckoutPageViewModel
    {
        private MainWindowViewModel mainWindowViewModel;

        public CheckoutPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
    }
}
