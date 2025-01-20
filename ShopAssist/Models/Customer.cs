using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    [Serializable]
    public class Customer
    {
        public string Name { get; set; }
        public Membership Membership { get; set; }
    }
}
