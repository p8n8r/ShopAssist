using ShopAssist.DisplayDialogs;
using ShopAssist.Models;
using ShopAssist.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;
using System.Xml.Serialization;

namespace ShopAssist.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private const string STORE_FILE = @".\Datasets\Store.xml";
        public readonly IDisplayDialog displayDialog;

        public Store Store { get; set; }

        private MainWindow mainWindow;
        public MainWindow MainWindow { get { return mainWindow; } }
        public RelayCommand customersCmd => new RelayCommand(execute => OpenCustomerPage());
        public RelayCommand inventoryCmd => new RelayCommand(execute => OpenInventoryPage());
        public RelayCommand onCloseCmd => new RelayCommand(execute => OnClose());

        public MainWindowViewModel(MainWindow mainWindow, IDisplayDialog displayDialog)
        {
            this.mainWindow = mainWindow;
            this.displayDialog = displayDialog;

            ImportStore(STORE_FILE);
            //AddJunkData(); // <---- REMOVE THIS 
        }

        private void AddJunkData()
        {
            //Add junk data
            this.Store.Customers = new List<Customer>()
            {
                new Customer() { Name = "Peyton Stults", Membership = new Membership() { Id = 1, MembershipLevel = MembershipLevel.High} },
                new Customer() { Name = "Addison Stults", Membership = new Membership() { Id = 2, MembershipLevel = MembershipLevel.Medium} },
                new Customer() { Name = "Parker Stults", Membership = new Membership() { Id = 3, MembershipLevel = MembershipLevel.Low} },
                new Customer() { Name = "Will Stults", Membership = new Membership() { Id = 4, MembershipLevel = MembershipLevel.None} }
            };

            this.Store.Inventory = new Dictionary<int, Item>()
            {
                { 0, new Item() { Name = "Apple", Category = "Fruit", Stock = 10, Price = 0.89M, Code = 0 } },
                { 1, new Item() { Name = "Steak", Category = "Meat", Stock = 3, Price = 8.99M, Code = 1 } },
                { 2, new Item() { Name = "Butter", Category = "Dairy", Stock = 5, Price = 3.56M, Code = 2 } }
            };
        }

        private void ImportStore(string filePath)
        {
            if (File.Exists(filePath))
                this.Store = ImportXmlFile(typeof(Store), filePath) as Store;

            if (this.Store != null) //Data found?
            {
                //Let's restore the inventory dictionay,
                //this time with a new ID number to get
                //around that IDictionary serialization issue.
                for (int i = 0; i < this.Store.Items.Count; i++)
                    this.Store.Inventory[i] = this.Store.Items[i];
            }
            else
            {
                this.Store = new Store(); //Create empty store
            }
        }

        private void ExportStore(string filePath)
        {
            if (this.Store != null) //Data found?
            {
                //Save Dictionary as List to avoid serialization problems 
                //with classes that implement IDictionary. We'll assign
                //new IDs on next startup to get around this issue.
                this.Store.Items = this.Store.Inventory.Select(p => p.Value).ToList();
            }
            else
            {
                this.Store = new Store(); //Create empty store
            }

            ExportAsXmlFile(this.Store, typeof(Store), filePath);
        }

        private object ImportXmlFile(Type dataType, string filePath)
        {
            object data = null;

            try
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer serializer = new XmlSerializer(dataType);

                using (stream)
                {
                    data = serializer.Deserialize(stream);
                    Convert.ChangeType(data, dataType);
                }
            }
            //catch (Exception e)
            //{
            //    displayDialog.ShowErrorMessageBox(e.Message);
            //    throw; //?
            //}
            catch (ArgumentNullException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentOutOfRangeException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (NotSupportedException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (FileNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (UnauthorizedAccessException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (DirectoryNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (PathTooLongException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (IOException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (SecurityException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (InvalidOperationException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (InvalidCastException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (FormatException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (OverflowException e) { displayDialog.ShowErrorMessageBox(e.Message); }

            return data;
        }

        private void ExportAsXmlFile(object data, Type dataType, string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            try
            {
                XmlSerializer serializer = new XmlSerializer(dataType);

                using (stream)
                {
                    serializer.Serialize(stream, data);
                }
            }
            //catch (Exception e)
            //{
            //    displayDialog.ShowErrorMessageBox(e.Message);
            //    throw; //?
            //}
            catch (ArgumentNullException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentOutOfRangeException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (ArgumentException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (NotSupportedException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (FileNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (UnauthorizedAccessException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (DirectoryNotFoundException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (PathTooLongException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (IOException e) { displayDialog.ShowErrorMessageBox(e.Message); }
            catch (SecurityException e) { displayDialog.ShowErrorMessageBox(e.Message); }
        }

        private void OpenCustomerPage()
        {
            this.mainWindow.mainFrame.Navigate(this.mainWindow.customerPage);
        }

        private void OpenInventoryPage()
        {
            this.mainWindow.mainFrame.Navigate(this.mainWindow.inventoryPage);
        }

        public Page GetCurrentPage()
        {
            if (this.MainWindow != null)
                return (this.MainWindow.FindName("mainFrame") as Frame).Content as Page;

            return null;
        }

        private void OnClose()
        {
            ExportStore(STORE_FILE);
        }
    }
}
