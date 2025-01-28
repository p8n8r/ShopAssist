using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.ViewModels
{
    internal class DirectionsPageViewModel : ViewModelBase
    {
        private MainWindowViewModel mainWindowViewModel;

        public DirectionsPageViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
    }
}
