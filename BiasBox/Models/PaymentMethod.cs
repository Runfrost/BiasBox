using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
      public class PaymentMethod
    {
        public int ID { get; set; } // Primärnyckel
        public string Name { get; set; } // Namn på betalningssätt (t.ex. Kort, Swish, Faktura)
        public string Description { get; set; } // Beskrivning av betalningssättet

    }
}
