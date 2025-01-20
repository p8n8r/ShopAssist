using ShopAssist.DisplayDialogs;
using ShopAssist.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public readonly IDisplayDialog displayDialog;
        private MainWindow mainWindow;
        public MainWindow MainWindow { get { return mainWindow; } }

        public MainWindowViewModel(MainWindow mainWindow, IDisplayDialog displayDialog)
        {
            this.mainWindow = mainWindow;
            this.displayDialog = displayDialog;
        }
    }
}
