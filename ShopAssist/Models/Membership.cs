using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    internal class Membership
    {
        public int ID { get; set; }
        public bool IsCurrentMember { get; set; } = false;
    }
}
