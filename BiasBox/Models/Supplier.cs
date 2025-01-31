using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Supplier
    {
        public int ID { get; set; } // Primärnyckel
        public string Name { get; set; } // Namn på leverantören
        public string ContactInfo { get; set; } // Kontaktinformation (mejl, telefon, osv.)
        public string Address { get; set; } // Adress till leverantören

        // Navigeringsreferens till produkter som leverantören tillhandahåller
        public List<Product> Products { get; set; } = new();

    }
}
