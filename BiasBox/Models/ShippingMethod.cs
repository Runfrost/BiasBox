using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class ShippingMethod
    {
        public int ID { get; set; } // Primärnyckel
        public string Name { get; set; } // Namn på fraktmetod (t.ex. Hemleverans, Postombud)
        public decimal Price { get; set; } // Kostnad för fraktmetoden
        public string EstimatedDeliveryTime { get; set; } // Uppskattad leveranstid (t.ex. "2-4 dagar")

    }
}
