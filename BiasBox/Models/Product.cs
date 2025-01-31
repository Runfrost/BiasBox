using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Product
    {

        public int ID { get; set; } 
        public string Name { get; set; }
        public string Info { get; set; }
        public decimal Price { get; set; }
        public int Inventory { get; set; }
        public bool IsFeatured { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }



        // Navigeringsreferenser
        public List<Group> Groups { get; set; } = new(); 
        public List<Artist> Artists { get; set; } = new();
        public List<Category> Categories { get; set; } = new(); 
        public List<CartItem> CartItems { get; set; } = new(); 
        public List<OrderItem> OrderItems { get; set; } = new();
       
    }
}
