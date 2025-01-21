using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShopAssist.Models
{
    [Serializable]
    public class Store
    {
        //private ObservableCollection<Item> items;
        public List<Customer> Customers { get; set; }
        public Tree<Category> Categories { get; set; }
        [XmlIgnore]
        public Dictionary<int, Item> Inventory { get; set; }
        public List<Item> Items { get; set; } //Only used for import/export

        //public ObservableCollection<Item> Items
        //{
        //    get { return this.items; }
        //    set { this.items = value; OnPropertyChanged(); }
        //}

        public Store()
        {
            this.Customers = new List<Customer>();
            this.Inventory = new Dictionary<int, Item>();
            this.Categories = new Tree<Category>();
        }
    }
}
