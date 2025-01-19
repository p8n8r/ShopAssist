using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Assist.Models
{
    internal class Store
    {
        public Dictionary<int, Item> Inventory { get; set; }
        public Tree<Category> Categories { get; set; }
    }
}
