﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    internal class Category
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }
    }
}
