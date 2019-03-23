using System;
using System.Collections.Generic;
using System.Text;

namespace MessageReceiverApp
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Customer { get; set; }
        public string ShippingAddress { get; set; }
        public int OrderAmount { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0}, {1}, {2}, {3}, {4}",
                this.OrderId, this.OrderDate.ToString(), this.Customer,
                this.ShippingAddress, this.OrderAmount);
        }
    }
}
