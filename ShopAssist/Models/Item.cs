using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Assist.Models
{
    internal class Item
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public Category Category { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        //public string Description { get; set; }
    }
}
