using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class OrderItem
    {

        public int ID { get; set; } // Primärnyckel (valfritt, EF kan klara sig utan)

        public int OrderId { get; set; } // Foreign Key till Order
        public Order Order { get; set; } // Navigeringsreferens

        public int ProductId { get; set; } // Foreign Key till Product
        public Product Product { get; set; } // Navigeringsreferens

        public int Quantity { get; set; } // Antal av produkten i ordern
        public decimal Price { get; set; } // Pris för produkten vid köptillfället

    }
}
