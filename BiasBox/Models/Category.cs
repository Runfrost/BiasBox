using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Category
    {

        public int ID { get; set; }
        public string Name  { get; set; }


        //En kategori kan ha flera produkter och en produkt kan finnas i flera kategorier
        public List<Product> Products { get; set; } = new();
    }
}
