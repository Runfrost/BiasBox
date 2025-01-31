using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Customer
    {
        public int ID { get; set; } // Primärnyckel
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }




        public Cart Cart { get; set; } // Navigeringsreferens till kundens varukorg
        public List<Order> Orders { get; set; } = new(); // Navigeringsreferens till Orders
    }
}
