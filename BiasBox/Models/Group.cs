using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Group
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime FormationDate { get; set; }


        // Navigeringsreferens för gruppens medlemmar
        public List<Artist> Members { get; set; } = new List<Artist>();

        // Navigeringsreferens för produkter
        public List<Product> Products { get; set; } = new();


    }
}
