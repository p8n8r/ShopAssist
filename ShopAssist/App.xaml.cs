using MediaKiosk.DisplayDialogs;
using Shop_Assist.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Shop_Assist
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IDisplayDialog displayDialog = new DisplayDialog();
            MainWindow mainWindow = new MainWindow(displayDialog);
            mainWindow.Show();
        }
    }
}
