using System.Windows;

namespace MediaKiosk.DisplayDialogs
{
    public interface IDisplayDialog
    {
        void ShowBasicMessageBox(string message);
        void ShowErrorMessageBox(string message);
    }

    public class DisplayDialog : IDisplayDialog
    {
        public void ShowBasicMessageBox(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowErrorMessageBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public class FakeDisplayDialog : IDisplayDialog
    {
        public void ShowBasicMessageBox(string message) { }
        public void ShowErrorMessageBox(string message) { }
    }
}
