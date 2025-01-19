using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop_Assist.Models
{
    internal class Category
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}
