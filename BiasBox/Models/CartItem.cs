using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class CartItem
    {
        public int ID { get; set; }


        //Navigeringspreferens till Cart
        public int CartId { get; set; }
        public Cart Cart { get; set; }


        public int ProductId { get; set; }
        public Product Product {get; set;}


        // Extra data för produkten i varukorgen (Antal av produkten)
        public int Quantity { get; set; } 


    }

}
