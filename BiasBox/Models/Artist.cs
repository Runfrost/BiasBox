using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Artist
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DebutDate { get; set; }
        public int? GroupId { get; set; } //Kan vara null eftersom en artisk kan vara solo och eftersom en artist bara kan vara i en grupp så är det en int och inte en string


        public Group Group { get; set; }
        public List<Product> Products { get; set; } = new();






    }
}
