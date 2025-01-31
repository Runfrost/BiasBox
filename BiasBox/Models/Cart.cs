using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Cart
    {

        public int ID { get; set; } 


        // Navigeringsreferens för många-till-många-relation via CartItem
        public List<CartItem> CartItems { get; set; } = new();


        // Navigeringsreferens för relationen till kunden
        public int CustomerId { get; set; } // Foreign Key
        public Customer Customer { get; set; } // Navigeringsreferens



    }
}
