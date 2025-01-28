using PommaLabs.Hippie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShopAssist.Models
{
    public class Register
    {
        private int totalQueuedCustomers = 0;
        public string Name { get; set; }
        public MultiHeap<QueuedCustomer> QueuedCustomers { get; private set; }

        public Register(string name)
        {
            this.Name = name;
            this.QueuedCustomers = HeapFactory.NewFibonacciHeap<QueuedCustomer>();
        }

        public void EnterCheckout(Customer customer)
        {
            QueuedCustomer queuedCustomer = new QueuedCustomer(customer)
            {
                Register = this,
                CheckoutEnteredTime = DateTime.Now
            };

            totalQueuedCustomers++;

            this.QueuedCustomers.Add(queuedCustomer);

            UpdateQueuedCustomerPositions();
        }

        private void UpdateQueuedCustomerPositions()
        {
            int position = 1; //Let's be one's based
            foreach (QueuedCustomer queuedCustomer in this.QueuedCustomers.OrderBy(c => c))
                queuedCustomer.Position = position++;
        }

        public QueuedCustomer Checkout()
        {
            if (this.QueuedCustomers.Count > 0)
            {
                QueuedCustomer queuedCustomer = this.QueuedCustomers.RemoveMin();
                queuedCustomer.CheckoutStartTime = DateTime.Now;

                UpdateQueuedCustomerPositions();

                return queuedCustomer;
            }
            return null;
        }

        public void LeaveCheckout(QueuedCustomer queuedCustomer)
        {
            queuedCustomer.CheckoutEndTime = DateTime.Now;
        }

        public bool AreCustomersWaiting()
        {
            return this.QueuedCustomers.Count > 0;
        }

        public static Register SelectLeastBusyRegister(Register[] registers)
        {
            int minCustomers = registers.Select(r => r.QueuedCustomers.Count).Min();
            return registers.First(r => r.QueuedCustomers.Count == minCustomers);
        }
    }
}
