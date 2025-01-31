using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Order
    {

        public int ID { get; set; } // Primärnyckel
        public int CustomerId { get; set; } // Foreign Key till Customer
        public string Status { get; set; } // Exempelvärden: Pending, Paid, Shipped
        public Customer Customer { get; set; } // Navigeringsreferens

        public DateTime OrderDate { get; set; } // Datum för beställningen
        public decimal TotalAmount { get; set; } // Totalbelopp för beställningen

        public List<OrderItem> OrderItems { get; set; } = new(); // Navigeringsreferens till OrderItems
        public int PaymentMethodId { get; set; } // Foreign Key
        public PaymentMethod PaymentMethod { get; set; } // Navigeringsreferens
        public int ShippingMethodId { get; set; } // Foreign Key
        public ShippingMethod ShippingMethod { get; set; } // Navigeringsreferens

    }
}
