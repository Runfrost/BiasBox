using BiasBox.Groups;
using BiasBox.Webshop.UI;
using Spectre.Console;
using System.Threading.Channels;
using System.Data.SqlClient; // För SqlConnection
using Dapper;
using BiasBox.Models;
using BiasBox.Admin;

namespace BiasBox
{
    internal class Program
    {

        private static Cart cart = new Cart { ID = 1, CustomerId = 1 };
        static void Main(string[] args)
        {


            Header.DisplayHeader();
            Space();
            StartPageCode.StartCode();


            while (true)
            {
                
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1: 
                        BTS.ShowBTSPage();
                        break;

                    case ConsoleKey.D2: 
                        Ateeze.ShowAteezPage();
                        break;

                    case ConsoleKey.C: 
                        CategoriesPage.ShowCategoriesPage();
                        break;

                    case ConsoleKey.K: 
                        CartPage.ShowCartPage();
                        break;

                    case ConsoleKey.L: 
                        CustomerPage.ShowCustomerPage();
                        break;

                    case ConsoleKey.H: 
                        Console.Clear();
                        Header.DisplayHeader();
                        Space();
                        StartPageCode.StartCode();
                        break;

                    case ConsoleKey.A: 
                        AdminPage.ShowAdminPage();
                        break;

                    default:
                       
                        break;
                }
            }



        }

        public static void Space()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }


        public static void Space2()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
           
        }



        // Try - Catch ligger i klassen AdminPage
        // Asynkrona anrop inkl tidtagning ligger i klassen ChangeProducts

    }
}
