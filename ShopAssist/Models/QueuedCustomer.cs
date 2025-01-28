using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    public class QueuedCustomer : Customer, IComparable<QueuedCustomer>
    {
        public QueuedCustomer(Customer customer)
        {
            this.Name = customer.Name;
            this.Membership = customer.Membership;
        }

        public Register Register { get; set; }
        public int Position { get; set; }
        public DateTime CheckoutEnteredTime { get; set; }
        public DateTime CheckoutStartTime { get; set; }
        public DateTime CheckoutEndTime { get; set; }

        public int CompareTo(QueuedCustomer other)
        {
            int priorityComparison = this.Membership.MembershipLevel.CompareTo(this.Membership.MembershipLevel);

            if (priorityComparison != 0)
                return priorityComparison;
            else
                return this.CheckoutEnteredTime.CompareTo(other.CheckoutEnteredTime);
        }
    }
}
