using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    public class Admin
    {

        public int ID { get; set; } // Primärnyckel
        public string Username { get; set; } // Användarnamn för admin
        public string Password { get; set; } // Lösenord för inloggning

        // Möjliga extra egenskaper
        public string FullName { get; set; } // Namn på admin
        public string Email { get; set; } // Mejl för kontakt

    }
}
