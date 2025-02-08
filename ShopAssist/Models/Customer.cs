using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    [Serializable]
    public class Customer : IComparable<Customer>
    {
        public string Name { get; set; }
        public Membership Membership { get; set; }

        public int CompareTo(Customer other)
        {
            return this.Membership.Id.CompareTo(other.Membership.Id);
        }
    }
}
